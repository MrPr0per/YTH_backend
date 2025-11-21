using MediatR;

namespace YTH_backend.Features.Events.Commands;

public record DeleteEventFromUserCommand(Guid RegistrationId, Guid CurrentUserId) : IRequest;