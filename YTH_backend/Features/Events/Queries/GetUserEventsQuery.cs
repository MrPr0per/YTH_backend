using MediatR;
using YTH_backend.DTOs.Event;
using YTH_backend.Enums;
using YTH_backend.Models;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Features.Events.Queries;

public record GetUserEventsQuery(Guid? UserId, int Take, OrderType OrderType, CursorType CursorType, string OrderFieldName, Guid? CursorId,  Guid? EventId, Guid CurrentUserId, bool IsAdmin) : IRequest<PagedResult<GetUserEventByIdResponseDto>>;