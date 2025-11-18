using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Event;
using YTH_backend.Features.Events.Commands;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Features.Events.Handlers;

public class PatchEventHandler(AppDbContext dbContext) : IRequestHandler<PatchEventCommand>
{
    public async Task Handle(PatchEventCommand request, CancellationToken cancellationToken)
    {
        var ev = await dbContext.Events.FindAsync([request.EventId], cancellationToken);
        
        if (ev == null)
            throw new EntityNotFoundException($"Event with id: {request.EventId} not found");

        var dto = new PatchEventRequestDto(ev.Name, ev.Description, ev.Type, ev.Date, ev.Address);
        
        request.Patch.ApplyTo(dto);
        
        if (dto.Name is not null)
            ev.Name = dto.Name;
        if (dto.Description is not null)
            ev.Description = dto.Description;
        if (dto.Type is not null)
            ev.Type = dto.Type.Value;
        if (dto.Date is not null)
            ev.Date = dto.Date.Value;
        if (dto.Address is not null)
            ev.Address = dto.Address;
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}