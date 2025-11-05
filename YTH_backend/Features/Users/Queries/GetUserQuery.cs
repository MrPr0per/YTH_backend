using MediatR;
using YTH_backend.DTOs.User;

namespace YTH_backend.Features.Users.Queries;

public record GetUserQuery(Guid Id) : IRequest<GetUserResponseDto>;