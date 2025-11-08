using MediatR;
using YTH_backend.DTOs.User;
using YTH_backend.Enums;
using YTH_backend.Models;

namespace YTH_backend.Features.Users.Queries;

public record GetAllNotificationsQuery(Guid Id, Guid CurrentUserId, int From, int Take, OrderType OrderType) : IRequest<PagedResult<GetNotificationsResponseDto>>;