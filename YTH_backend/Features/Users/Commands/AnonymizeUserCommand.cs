using MediatR;

namespace YTH_backend.Features.Users.Commands;

public record AnonymizeUserCommand(Guid UserId) : IRequest;