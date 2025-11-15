using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.User;
using YTH_backend.Features.Users.Queries;
using YTH_backend.Models.User;

namespace YTH_backend.Features.Users.Handlers;

public class GetNotificationHandler(AppDbContext dbContext) : IRequestHandler<GetNotificationQuery, GetNotificationsResponseDto>
{
    public async Task<GetNotificationsResponseDto> Handle(GetNotificationQuery request, CancellationToken cancellationToken)
    {
        if (request.CurrentUserId != request.UserId)
            throw new UnauthorizedAccessException("User does not have permission to view other users notifications");

        var notification = await dbContext.Notifications.FindAsync([request.NotificationId], cancellationToken);

        if (notification != null)
            return new GetNotificationsResponseDto(notification.Title, notification.NotificationText,
                notification.CreatedAt, notification.IsRead);
        
        throw new KeyNotFoundException($"Notification with id:{request.NotificationId} not found");
    }
}