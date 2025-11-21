using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.Features.Courses.Commands;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Features.Courses.Handlers;

public class DeleteCourseFromUserHandler(AppDbContext dbContext) : IRequestHandler<DeleteCourseFromUserCommand>
{
    public async Task Handle(DeleteCourseFromUserCommand request, CancellationToken cancellationToken)
    {
        var registration =
            await dbContext.UserCourseRegistrations.FindAsync([request.RegistrationId], cancellationToken);
        
        if (registration == null)
            throw new EntityNotFoundException($"Course registration with id:{request.RegistrationId} was not found");
        
        if (registration.UserId != request.CurrentUserId)
            throw new UnauthorizedAccessException("You do not have permission to delete other user's course");
        
        dbContext.UserCourseRegistrations.Remove(registration);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}