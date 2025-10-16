using MediatR;
using YTH_backend.DTOs.Event;
using YTH_backend.Enums;
using YTH_backend.Models;

namespace YTH_backend.Features.Events.Queries;

public record GetAllEventsQuery(int From, int Take, OrderType OrderType) : IRequest<PagedResult<GetEventsResponseDto>>;