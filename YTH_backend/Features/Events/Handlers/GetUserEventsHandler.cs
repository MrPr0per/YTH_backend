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
        var user = await dbContext.Users
            .Include(u => u.Events)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        
        if (user == null)
            throw new KeyNotFoundException($"User with id: {request.UserId} not found");

        var query = request.OrderType == OrderType.Asc
            ? user.Events.OrderBy(e => e.Date)
            : user.Events.OrderByDescending(e => e.Date);

        var data = query
            .Skip(request.From - 1)
            .Take(request.Take)
            .Select(ev => new GetEventResponseDto(
                ev.Name,
                ev.Description,
                ev.ShortDescription,
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