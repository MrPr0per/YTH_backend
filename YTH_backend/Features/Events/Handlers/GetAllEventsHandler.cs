using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Event;
using YTH_backend.Features.Events.Queries;
using YTH_backend.Models;

namespace YTH_backend.Features.Events.Handlers;

public class GetAllEventsHandler(AppDbContext context) : IRequestHandler<GetAllEventsQuery, PagedResult<GetEventResponseDto>>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task<PagedResult<GetEventResponseDto>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}