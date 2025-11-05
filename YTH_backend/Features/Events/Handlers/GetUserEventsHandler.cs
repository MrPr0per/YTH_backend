using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.Event;
using YTH_backend.Enums;
using YTH_backend.Features.Events.Queries;
using YTH_backend.Models;

namespace YTH_backend.Features.Events.Handlers;

public class GetUserEventsHandler(AppDbContext context) : IRequestHandler<GetUserEventsQuery, PagedResult<GetEventResponseDto>>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task<PagedResult<GetEventResponseDto>> Handle(GetUserEventsQuery request, CancellationToken cancellationToken)
    {
        var userExists = await dbContext.Users
            .AnyAsync(u => u.Id == request.UserId, cancellationToken);

        if (!userExists)
            throw new KeyNotFoundException($"User with id: {request.UserId} not found");

        var from = request.From < 1 ? 1 : request.From;
        var skip = from - 1;

        var eventsQuery = dbContext.UserEventRegistrations
            .Where(r => r.UserId == request.UserId)
            .Select(r => r.Event)
            .AsQueryable();
        
        var query = request.OrderType == OrderType.Asc
            ? eventsQuery.OrderBy(e => e.Date)
            : eventsQuery.OrderByDescending(e => e.Date);

        var data = query
            .Skip(skip)
            .Take(request.Take)
            .Select(ev => new GetEventResponseDto(
                ev.Name,
                ev.Description,
                ev.Type,
                ev.Date,
                ev.Address))
            .ToList();
        
        return new PagedResult<GetEventResponseDto>(
            request.From,
            request.Take,
            request.OrderType,
            data);
    }
}