using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Event;
using YTH_backend.Features.Events.Commands;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Infrastructure.Object_storage;

namespace YTH_backend.Features.Events.Handlers;

public class PatchEventHandler(AppDbContext dbContext, ImageAdder imageAdder, IStorageService storageService) : IRequestHandler<PatchEventCommand>
{
    public async Task Handle(PatchEventCommand request, CancellationToken cancellationToken)
    {
        var ev = await dbContext.Events.FindAsync([request.EventId], cancellationToken);
        
        if (ev == null)
            throw new EntityNotFoundException($"Event with id: {request.EventId} not found");

        var dto = new PatchEventRequestDto(ev.Name, ev.Description, ev.Type, ev.Date, ev.Address, ev.ImageUrl);
        
        request.Patch.ApplyTo(dto);
        
        if (dto.Name is not null)
            ev.Name = dto.Name;
        if (dto.Description is not null)
            ev.Description = dto.Description;
        if (dto.Type is not null)
            ev.Type = dto.Type.Value;
        if (dto.Date is not null)
            ev.Date = dto.Date.Value;
        ev.Address = dto.Address;
        if (Base64Helper.IsBase64String(dto.ImageBase64!) || dto.ImageBase64 is null)
        {
            if (ev.ImageUrl is not null)
                await storageService.DeleteByUrlAsync(ev.ImageUrl, cancellationToken);
            
            var url = null as string;
            
            if (dto.ImageBase64 is not null)
                url = await imageAdder.AddImageToObjectStorage(dto.ImageBase64, $"event_{ev.Id}_{DateTime.UtcNow:yyyyMMddHHmmss}", true);
            
            ev.ImageUrl = url;
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}