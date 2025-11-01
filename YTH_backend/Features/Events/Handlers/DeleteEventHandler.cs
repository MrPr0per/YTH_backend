using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Events.Commands;


namespace YTH_backend.Features.Events.Handlers;

public class DeleteEventHandler(AppDbContext context) : IRequestHandler<DeleteEventCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var ev = await dbContext.Events.FindAsync(request.EventId, cancellationToken);
        
        if (ev == null)
            throw new KeyNotFoundException($"Event with id: {request.EventId} not found");
        
        dbContext.Events.Remove(ev);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}