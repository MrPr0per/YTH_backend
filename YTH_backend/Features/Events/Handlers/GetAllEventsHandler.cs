using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.Event;
using YTH_backend.Enums;
using YTH_backend.Features.Events.Queries;
using YTH_backend.Infrastructure;
using YTH_backend.Models;
using YTH_backend.Models.Event;
using YTH_backend.Models.Infrastructure;
using YTH_backend.Models.Post;

namespace YTH_backend.Features.Events.Handlers;

public class GetAllEventsHandler(AppDbContext dbContext) : IRequestHandler<GetAllEventsQuery, PagedResult<GetEventResponseDto>>
{
    public async Task<PagedResult<GetEventResponseDto>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Events
            .ApplyOrderSettings(request.OrderType, request.OrderFieldName)
            .ApplyCursorSettings(request.CursorType, request.Take, request.CursorId);
        
        var data = await query
            .Select(ev => new GetEventResponseDto(
                ev.Name,
                ev.Description,
                ev.Type,
                ev.Date,
                ev.Address))
            .ToListAsync(cancellationToken);

        return new PagedResult<GetEventResponseDto>(
            request.Take,
            request.OrderFieldName,
            request.OrderType,
            request.CursorType,
            data);
    }
}