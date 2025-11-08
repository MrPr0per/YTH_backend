using MediatR;

namespace YTH_backend.Features.Users.Commands;

public record ReadNotificationCommand(Guid UserId, Guid CurrentUserId, Guid NotificationId) : IRequest;