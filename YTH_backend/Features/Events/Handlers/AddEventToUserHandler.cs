using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.Features.Events.Commands;

namespace YTH_backend.Features.Events.Handlers;

public class AddEventToUserHandler(AppDbContext context) : IRequestHandler<AddEventToUserCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(AddEventToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .Include(u => u.Events) 
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null) 
            throw new KeyNotFoundException($"User with id: {request.UserId} not found");

        var ev = await dbContext.Events
            .Include(e => e.Users) 
            .FirstOrDefaultAsync(e => e.Id == request.EventId, cancellationToken);

        if (ev == null)
            throw new KeyNotFoundException($"Event with id: {request.EventId} not found");

        if (!user.Events.Any(e => e.Id == ev.Id))
        {
            user.Events.Add(ev);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}