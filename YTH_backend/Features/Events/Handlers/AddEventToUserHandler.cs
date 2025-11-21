using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.Event;
using YTH_backend.Features.Events.Commands;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models.Event;
using YTH_backend.Models.User;

namespace YTH_backend.Features.Events.Handlers;

public class AddEventToUserHandler(AppDbContext dbContext) : IRequestHandler<AddEventToUserCommand,  AddEventToUserResponseDto>
{
    public async Task<AddEventToUserResponseDto> Handle(AddEventToUserCommand request,
        CancellationToken cancellationToken)
    {
        if (request.CurrentUserId != request.UserId)
            throw new UnauthorizedAccessException();
        
        var userExists = await dbContext.Users
            .AnyAsync(u => u.Id == request.UserId, cancellationToken);

        if (!userExists)
            throw new EntityNotFoundException($"User with id: {request.UserId} not found");

        var eventExists = await dbContext.Events
            .AnyAsync(e => e.Id == request.EventId, cancellationToken);

        if (eventExists)
            throw new EntityNotFoundException($"Event with id: {request.EventId} not found");

        var alreadyRegistered = await dbContext.UserEventRegistrations
            .AnyAsync(r => r.UserId == request.UserId && r.EventId == request.EventId, cancellationToken);

        if (alreadyRegistered)
            throw new EntityAlreadyExistsException(
                $"Event with id: {request.EventId} is already registered to user with id: {request.UserId}");

        var registration = new UserEventRegistration
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            EventId = request.EventId,
        };

        dbContext.UserEventRegistrations.Add(registration);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AddEventToUserResponseDto(registration.Id, registration.CreatedAt, registration.UserId,
            registration.EventId);
    }
}