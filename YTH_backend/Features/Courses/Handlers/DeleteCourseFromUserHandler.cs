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
        var user = await dbContext.Users
            .Include(u => u.Courses) 
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null) 
            throw new KeyNotFoundException($"User with id: {request.UserId} not found");
        
        var course = await dbContext.Courses
            .Include(c => c.Users)
            .FirstOrDefaultAsync(c => c.Id == request.CourseId, cancellationToken);
        
        if (course == null)
            throw new KeyNotFoundException($"Course with id: {request.CourseId} not found");

        if (user.Courses.Any(c => c.Id == course.Id))
        {
            user.Courses.Remove(course);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}