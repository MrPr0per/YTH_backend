using MediatR;
using YTH_backend.DTOs.User;
using YTH_backend.Enums;
using YTH_backend.Models;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Features.Users.Queries;

public record GetAllNotificationsQuery(Guid Id, Guid CurrentUserId, int Take, OrderType OrderType, CursorType CursorType, string OrderFieldName, Guid? CursorId) : IRequest<PagedResult<GetNotificationsResponseDto>>;