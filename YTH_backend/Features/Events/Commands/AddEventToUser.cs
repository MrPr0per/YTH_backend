using MediatR;

namespace YTH_backend.Features.Events.Commands;

public record AddEventToUser(Guid UserId, Guid EventId) : IRequest;