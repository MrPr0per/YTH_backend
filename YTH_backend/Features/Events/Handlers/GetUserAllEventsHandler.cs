using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.Event;
using YTH_backend.Enums;
using YTH_backend.Features.Events.Queries;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Features.Events.Handlers;

public class GetUserAllEventsHandler(AppDbContext dbContext) : IRequestHandler<GetUserEventsQuery, PagedResult<GetUserEventByIdResponseDto>>
{
    public async Task<PagedResult<GetUserEventByIdResponseDto>> Handle(GetUserEventsQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId != null)
        {
            var userExists = await dbContext.Users
                .AnyAsync(u => u.Id == request.UserId, cancellationToken);

            if (!userExists)
                throw new EntityNotFoundException($"User with id: {request.UserId} not found");
            
            if (request.UserId != request.CurrentUserId)
                throw new UnauthorizedAccessException("You can't read other users registrations");
        }
        else
        {
            if (!request.IsAdmin)
                throw new UnauthorizedAccessException("You can't read other users");
        }
        
        var eventsQuery = dbContext.UserEventRegistrations.AsQueryable();
        
        if (request.EventId != null)
            eventsQuery = eventsQuery.Where(e => e.EventId == request.EventId);
        
        if (request.UserId != null)
            eventsQuery = eventsQuery.Where(e => e.UserId == request.UserId);
        
        var take = request.Take;

        if (take < 0)
            take = 10;
        
        var query = eventsQuery
            .ApplyOrderSettings(request.OrderType, request.OrderFieldName)
            .ApplyCursorSettings(request.CursorType, take, request.CursorId);

        var data = await query
            .Select(ev => new GetUserEventByIdResponseDto(
                ev.Id,
                ev.CreatedAt,
                ev.UserId,
                ev.EventId))
            .ToListAsync(cancellationToken);
        
        return new PagedResult<GetUserEventByIdResponseDto>(
            take,
            request.OrderFieldName,
            request.OrderType,
            request.CursorType,
            data);
    }
}