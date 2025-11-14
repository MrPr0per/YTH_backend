using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Events.Commands;
using YTH_backend.Models.Event;

namespace YTH_backend.Features.Events.Handlers;

public class AddEventHandler(AppDbContext dbContext) : IRequestHandler<AddEventCommand>
{
    public async Task Handle(AddEventCommand request, CancellationToken cancellationToken)
    {
        var newEvent = new Event
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Date = request.Date,
            Type = request.Type,
            Address = request.Address
        };
        
        await dbContext.Events.AddAsync(newEvent, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken); 
    }
}