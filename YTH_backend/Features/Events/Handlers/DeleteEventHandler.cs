using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Events.Commands;
using YTH_backend.Infrastructure.Exceptions;


namespace YTH_backend.Features.Events.Handlers;

public class DeleteEventHandler(AppDbContext dbContext) : IRequestHandler<DeleteEventCommand>
{
    public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var ev = await dbContext.Events.FindAsync([request.EventId], cancellationToken);
        
        if (ev == null)
            throw new EntityNotFoundException($"Event with id: {request.EventId} not found");
        
        dbContext.Events.Remove(ev);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}