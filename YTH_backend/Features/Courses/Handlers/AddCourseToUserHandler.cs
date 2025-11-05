using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.Features.Courses.Commands;
using YTH_backend.Models.User;

namespace YTH_backend.Features.Courses.Handlers;

public class AddCourseToUserHandler(AppDbContext context) : IRequestHandler<AddCourseToUserCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(AddCourseToUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await dbContext.Users
            .AnyAsync(u => u.Id == request.UserId, cancellationToken);

        if (!userExists)
            throw new KeyNotFoundException($"User with id: {request.UserId} not found");
        
        var courseExists = await dbContext.Courses
            .AnyAsync(c => c.Id == request.CourseId, cancellationToken);

        if (!courseExists)
            throw new KeyNotFoundException($"Course with id: {request.CourseId} not found");
        
        var alreadyRegistered = await dbContext.UserCourseRegistrations
            .AnyAsync(r => r.UserId == request.UserId && r.CourseId == request.CourseId, cancellationToken);

        if (!alreadyRegistered)
        {
            var registration = new UserCourseRegistration
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                CourseId = request.CourseId,
            };

            dbContext.UserCourseRegistrations.Add(registration);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        
    }
}