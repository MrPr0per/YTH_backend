using MediatR;

namespace YTH_backend.Features.Users.Commands;

public record AuthUserCommand(string Login, string Password) : IRequest;