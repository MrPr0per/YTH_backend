using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.Event;
using YTH_backend.Enums;
using YTH_backend.Features.Events.Queries;
using YTH_backend.Models;
using YTH_backend.Models.Event;
using YTH_backend.Models.Post;

namespace YTH_backend.Features.Events.Handlers;

public class GetAllEventsHandler(AppDbContext context) : IRequestHandler<GetAllEventsQuery, PagedResult<GetEventResponseDto>>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task<PagedResult<GetEventResponseDto>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Event> query = dbContext.Events;
        
        query = request.OrderType == OrderType.Asc
            ? query.OrderBy(x => x.Date)
            : query.OrderByDescending(x => x.Date);
        
        var data = await query
            .Skip(request.From - 1)
            .Take(request.Take)
            .Select(ev => new GetEventResponseDto(
                ev.Name,
                ev.Description,
                ev.Type,
                ev.Date,
                ev.Address))
            .ToListAsync(cancellationToken);

        return new PagedResult<GetEventResponseDto>(
            request.From,
            request.Take,
            request.OrderType,
            data);
    }
}