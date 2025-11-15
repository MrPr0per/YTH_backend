using MediatR;
using YTH_backend.DTOs.User;

namespace YTH_backend.Features.Users.Commands;

public record LoginUserCommand(string Login, string Password) : IRequest<LoginUserResponseDto>;