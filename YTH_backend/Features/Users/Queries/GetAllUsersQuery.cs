using MediatR;
using YTH_backend.DTOs.User;
using YTH_backend.Enums;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Features.Users.Queries;

public record GetAllUsersQuery(int Take, OrderType OrderType, CursorType CursorType, string OrderFieldName, Guid? CursorId) : IRequest<PagedResult<GetAllUsersResponseDto>>;