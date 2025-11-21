using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.Features.Events.Commands;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Features.Events.Handlers;

public class DeleteEventFromUserHandler(AppDbContext dbContext) : IRequestHandler<DeleteEventFromUserCommand>
{
    public async Task Handle(DeleteEventFromUserCommand request, CancellationToken cancellationToken)
    {
        var registration = await dbContext.UserEventRegistrations.FindAsync([request.RegistrationId], cancellationToken);
        
        if (registration == null)
            throw new EntityNotFoundException($"Event registration with id:{request.RegistrationId} was not found");
        
        if (registration.UserId != request.CurrentUserId)
            throw new UnauthorizedAccessException("You do not have permission to delete other user's event");
        
        dbContext.UserEventRegistrations.Remove(registration);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}