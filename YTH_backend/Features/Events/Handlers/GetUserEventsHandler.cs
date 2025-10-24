using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Event;
using YTH_backend.Features.Events.Queries;
using YTH_backend.Models;

namespace YTH_backend.Features.Events.Handlers;

public class GetUserEventsHandler(AppDbContext context) : IRequestHandler<GetUserEventsQuery, PagedResult<GetEventResponseDto>>
{
    private readonly AppDbContext dbContext = context;
    
    public Task<PagedResult<GetEventResponseDto>> Handle(GetUserEventsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}