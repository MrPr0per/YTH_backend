using MediatR;
using YTH_backend.DTOs.Event;

namespace YTH_backend.Features.Events.Commands;

public record AddEventToUserCommand(Guid UserId, Guid EventId, Guid CurrentUserId) : IRequest<AddEventToUserResponseDto>;