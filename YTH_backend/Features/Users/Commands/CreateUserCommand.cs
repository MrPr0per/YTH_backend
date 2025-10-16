using MediatR;
using YTH_backend.DTOs.User;

namespace YTH_backend.Features.Users.Commands;

public record CreateUserCommand(string UserName, string Password, string JwtEmail) : IRequest;