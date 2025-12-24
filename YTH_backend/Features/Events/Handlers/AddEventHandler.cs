using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Event;
using YTH_backend.Features.Events.Commands;
using YTH_backend.Infrastructure.Object_storage;
using YTH_backend.Models.Event;

namespace YTH_backend.Features.Events.Handlers;

public class AddEventHandler(AppDbContext dbContext, ImageAdder imageAdder) : IRequestHandler<AddEventCommand, AddEventResponseDto>
{
    public async Task<AddEventResponseDto> Handle(AddEventCommand request, CancellationToken cancellationToken)
    {
        var imageUrl = null as string;
        var eventId = Guid.NewGuid();
        
        if (request.ImageBase64 != null)
            imageUrl = await imageAdder.AddImageToObjectStorage(request.ImageBase64,
                $"event_{eventId}_{DateTime.UtcNow:yyyyMMddHHmmss}", true);
        
        var newEvent = new Event
        {
            Id = eventId,
            Name = request.Name,
            Description = request.Description,
            Date = request.Date,
            Type = request.Type,
            Address = request.Address,
            ImageUrl = imageUrl,
        };
        
        await dbContext.Events.AddAsync(newEvent, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AddEventResponseDto(newEvent.Id, newEvent.Name, newEvent.Description, newEvent.Date, newEvent.Type,
            newEvent.Address, newEvent.ImageUrl);
    }
}