using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.Features.Events.Commands;

namespace YTH_backend.Features.Events.Handlers;

public class DeleteEventFromUserHandler(AppDbContext context) : IRequestHandler<DeleteEventFromUserCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(DeleteEventFromUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await dbContext.Users
            .AnyAsync(u => u.Id == request.UserId, cancellationToken);

        if (!userExists)
            throw new KeyNotFoundException($"User with id: {request.UserId} not found");

        var eventExists = await dbContext.Events
            .AnyAsync(e => e.Id == request.EventId, cancellationToken);

        if (eventExists)
            throw new KeyNotFoundException($"Event with id: {request.EventId} not found");
        
        var registration = await dbContext.UserEventRegistrations
            .FirstOrDefaultAsync(r => r.EventId == request.EventId && r.UserId == request.UserId, cancellationToken);
        
        if (registration != null)
        {
            dbContext.UserEventRegistrations.Remove(registration);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}