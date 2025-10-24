using MediatR;

namespace YTH_backend.Features.Users.Commands;

public record LoginUserCommand(string Login, string Password) : IRequest;