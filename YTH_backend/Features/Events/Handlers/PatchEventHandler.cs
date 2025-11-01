using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Event;
using YTH_backend.Features.Events.Commands;

namespace YTH_backend.Features.Events.Handlers;

public class PatchEventHandler(AppDbContext context) : IRequestHandler<PatchEventCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(PatchEventCommand request, CancellationToken cancellationToken)
    {
        var ev = await dbContext.Events.FindAsync(request.EventId, cancellationToken);
        
        if (ev == null)
            throw new KeyNotFoundException($"Event with id: {request.EventId} not found");

        var dto = new PatchEventRequestDto(ev.Name, ev.Description, ev.ShortDescription, ev.Type, ev.Date, ev.Address);
        
        request.Patch.ApplyTo(dto);
        
        if (dto.Name is not null)
            ev.Name = dto.Name;
        if (dto.Description is not null)
            ev.Description = dto.Description;
        if (dto.ShortDescription is not null)
            ev.ShortDescription = dto.ShortDescription;
        if (dto.Type is not null)
            ev.Type = dto.Type.Value;
        if (dto.Date is not null)
            ev.Date = dto.Date.Value;
        if (dto.Address is not null)
            ev.Address = dto.Address;
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}