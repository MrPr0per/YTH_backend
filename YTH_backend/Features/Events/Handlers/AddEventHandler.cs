using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Events.Commands;
using YTH_backend.Models.Event;

namespace YTH_backend.Features.Events.Handlers;

public class AddEventHandler(AppDbContext context) : IRequestHandler<AddEventCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(AddEventCommand request, CancellationToken cancellationToken)
    {
        var newEvent = new Event
        {
            Name = request.Name,
            Description = request.Description,
            Date = request.Date,
            ShortDescription = request.ShortDescription,
            Type = request.Type,
            Address = request.Address
        };
        
        await dbContext.Events.AddAsync(newEvent, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken); 
    }
}