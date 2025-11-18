using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.Event;
using YTH_backend.Enums;
using YTH_backend.Features.Events.Queries;
using YTH_backend.Infrastructure;
using YTH_backend.Models;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Features.Events.Handlers;

public class GetUserEventsHandler(AppDbContext dbContext) : IRequestHandler<GetUserEventsQuery, PagedResult<GetEventResponseDto>>
{
    public async Task<PagedResult<GetEventResponseDto>> Handle(GetUserEventsQuery request, CancellationToken cancellationToken)
    {
        var userExists = await dbContext.Users
            .AnyAsync(u => u.Id == request.UserId, cancellationToken);

        if (!userExists)
            throw new KeyNotFoundException($"User with id: {request.UserId} not found");

        var eventsQuery = dbContext.UserEventRegistrations
            .Where(r => r.UserId == request.UserId)
            .Select(r => r.Event)
            .AsQueryable();
        
        var query = eventsQuery
            .ApplyOrderSettings(request.OrderType, request.OrderFieldName)
            .ApplyCursorSettings(request.CursorType, request.Take, request.CursorId);

        var data = query
            .Select(ev => new GetEventResponseDto(
                ev.Name,
                ev.Description,
                ev.Type,
                ev.Date,
                ev.Address))
            .ToList();
        
        return new PagedResult<GetEventResponseDto>(
            request.Take,
            request.OrderFieldName,
            request.OrderType,
            request.CursorType,
            data);
    }
}