using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Events.Commands;

namespace YTH_backend.Features.Events.Handlers;

public class PatchEventHandler(AppDbContext context) : IRequestHandler<PatchEventCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(PatchEventCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}