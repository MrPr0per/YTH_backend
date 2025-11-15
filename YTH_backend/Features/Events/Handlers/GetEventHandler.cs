using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Event;
using YTH_backend.Features.Events.Queries;

namespace YTH_backend.Features.Events.Handlers;

public class GetEventHandler(AppDbContext dbContext) : IRequestHandler<GetEventQuery, GetEventResponseDto>
{
    public async Task<GetEventResponseDto> Handle(GetEventQuery request, CancellationToken cancellationToken)
    {
        var ev = await dbContext.Events.FindAsync([request.EventId], cancellationToken);
        
        if (ev == null)
            throw new KeyNotFoundException($"Event with id {request.EventId} not found");

        return new GetEventResponseDto(
            ev.Name,
            ev.Description,
            ev.Type,
            ev.Date,
            ev.Address);
    }
}