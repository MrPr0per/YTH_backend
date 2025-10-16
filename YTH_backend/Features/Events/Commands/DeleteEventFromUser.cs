using MediatR;

namespace YTH_backend.Features.Events.Commands;

public record DeleteEventFromUser(Guid UserId, Guid EventId) : IRequest;