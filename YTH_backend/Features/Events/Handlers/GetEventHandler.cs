using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Event;
using YTH_backend.Features.Events.Queries;

namespace YTH_backend.Features.Events.Handlers;

public class GetEventHandler(AppDbContext context) : IRequestHandler<GetEventQuery, GetEventsResponseDto>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task<GetEventsResponseDto> Handle(GetEventQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}