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
        var take = request.Take;

        if (take < 0)
            take = 10;
        
        var query = dbContext.Events
            .ApplyOrderSettings(request.OrderType, request.OrderFieldName)
            .ApplyCursorSettings(request.CursorType, take, request.CursorId);
        
        var data = await query
            .Select(ev => new GetEventResponseDto(
                ev.Id,
                ev.Name,
                ev.Description,
                ev.Type,
                ev.Date,
                ev.Address,
                ev.ImageUrl))
            .ToListAsync(cancellationToken);

        return new PagedResult<GetEventResponseDto>(
            take,
            request.OrderFieldName,
            request.OrderType,
            request.CursorType,
            data);
    }
}