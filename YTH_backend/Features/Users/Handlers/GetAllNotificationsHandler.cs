using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.User;
using YTH_backend.Enums;
using YTH_backend.Features.Users.Queries;
using YTH_backend.Infrastructure;
using YTH_backend.Models;
using YTH_backend.Models.Infrastructure;
using YTH_backend.Models.User;

namespace YTH_backend.Features.Users.Handlers;

public class GetAllNotificationsHandler(AppDbContext dbContext) : IRequestHandler<GetAllNotificationsQuery, PagedResult<GetNotificationsResponseDto>>
{
    public async Task<PagedResult<GetNotificationsResponseDto>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
    {
        if (request.CurrentUserId != request.Id)
            throw new UnauthorizedAccessException("User does not have permission to view other users notifications");

        var take = request.Take;
        
        if (take < 0)
            take = 10;
        
        var query = dbContext.Notifications
            .ApplyOrderSettings(request.OrderType, request.OrderFieldName)
            .ApplyCursorSettings(request.CursorType, take, request.CursorId);
        
        var data = await query
            .Select(n => new GetNotificationsResponseDto(
                n.Id,
                n.Title,
                n.NotificationText,
                n.CreatedAt,
                n.IsRead,
                n.UserId))
            .ToListAsync(cancellationToken);

        return new PagedResult<GetNotificationsResponseDto>(
            take,
            request.OrderFieldName,
            request.OrderType,
            request.CursorType,
            data);
    }
}