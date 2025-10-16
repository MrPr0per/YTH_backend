using MediatR;

namespace YTH_backend.Features.Events.Commands;

public record AddEventToUserCommand(Guid UserId, Guid EventId) : IRequest;