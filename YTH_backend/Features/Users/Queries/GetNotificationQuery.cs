using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YTH_backend.DTOs.User;

namespace YTH_backend.Features.Users.Queries;

public record GetNotificationQuery(Guid UserId, Guid CurrentUserId, Guid NotificationId) : IRequest<GetNotificationsResponseDto>;