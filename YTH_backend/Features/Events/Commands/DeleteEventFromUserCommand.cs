using MediatR;

namespace YTH_backend.Features.Events.Commands;

public record DeleteEventFromUserCommand(Guid UserId, Guid EventId) : IRequest;