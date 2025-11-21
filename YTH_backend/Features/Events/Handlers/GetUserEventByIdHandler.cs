using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Event;
using YTH_backend.Features.Events.Queries;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Features.Events.Handlers;

public class GetUserEventByIdHandler(AppDbContext dbContext) : IRequestHandler<GetUserEventByIdQuery,  GetUserEventByIdResponseDto>
{
    public async Task<GetUserEventByIdResponseDto> Handle(GetUserEventByIdQuery request, CancellationToken cancellationToken)
    {
        var registration = await dbContext.UserEventRegistrations.FindAsync([request.RegistrationId], cancellationToken);
        
        if (registration == null)
            throw new EntityNotFoundException($"Event registration with id:{request.RegistrationId} was not found");
        
        if (registration.UserId != request.CurrentUserId)
            throw new UnauthorizedAccessException("You do not have permission to see other user's event");

        return new GetUserEventByIdResponseDto(registration.Id, registration.CreatedAt, registration.UserId,
            registration.EventId);
    }
}