using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Events.Commands;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Infrastructure.Object_storage;


namespace YTH_backend.Features.Events.Handlers;

public class DeleteEventHandler(AppDbContext dbContext, IStorageService storageService) : IRequestHandler<DeleteEventCommand>
{
    public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var ev = await dbContext.Events.FindAsync([request.EventId], cancellationToken);
        
        if (ev == null)
            throw new EntityNotFoundException($"Event with id: {request.EventId} not found");
        
        if (ev.ImageUrl != null)
            await storageService.DeleteByUrlAsync(ev.ImageUrl, cancellationToken);
        
        dbContext.Events.Remove(ev);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}