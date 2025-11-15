using MediatR;
using YTH_backend.DTOs.User;

namespace YTH_backend.Features.Users.Commands;

public record RefreshCommand() : IRequest<RefreshTokenResponseDto>;