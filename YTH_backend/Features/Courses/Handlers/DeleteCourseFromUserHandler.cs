using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.Features.Courses.Commands;

namespace YTH_backend.Features.Courses.Handlers;

public class DeleteCourseFromUserHandler(AppDbContext context) : IRequestHandler<DeleteCourseFromUserCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(DeleteCourseFromUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await dbContext.Users
            .AnyAsync(u => u.Id == request.UserId, cancellationToken);

        if (!userExists)
            throw new KeyNotFoundException($"User with id: {request.UserId} not found");
        
        var courseExists = await dbContext.Courses
            .AnyAsync(c => c.Id == request.CourseId, cancellationToken);

        if (!courseExists)
            throw new KeyNotFoundException($"Course with id: {request.CourseId} not found");
        
        var registration = await dbContext.UserCourseRegistrations
            .FirstOrDefaultAsync(r => r.UserId == request.UserId && r.CourseId == request.CourseId, cancellationToken);

        if (registration != null)
        {
            dbContext.UserCourseRegistrations.Remove(registration);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}