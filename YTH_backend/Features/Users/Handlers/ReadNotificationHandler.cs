using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.User;
using YTH_backend.Features.Users.Commands;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Features.Users.Handlers;

public class ReadNotificationHandler(AppDbContext dbContext) : IRequestHandler<ReadNotificationCommand>
{
    public async Task Handle(ReadNotificationCommand request, CancellationToken cancellationToken)
    {
        if (request.CurrentUserId != request.UserId)
            throw new UnauthorizedAccessException("User does not have permission to view other users notifications");

        var notification = await dbContext.Notifications.FindAsync([request.NotificationId], cancellationToken);

        if (notification == null)
            throw new EntityNotFoundException($"Notification with id:{request.NotificationId} not found");
        
        if (notification.IsRead)
            throw new InvalidOperationException("Notification is already read");
        
        notification.IsRead = true;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}