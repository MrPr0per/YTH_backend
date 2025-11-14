using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Courses.Commands;
using YTH_backend.Features.Events.Commands;

namespace YTH_backend.Features.Courses.Handlers;

public class DeleteCourseHandler(AppDbContext dbContext) : IRequestHandler<DeleteCourseCommand>
{
    public async Task Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await dbContext.Courses.FindAsync(request.CourseId, cancellationToken);
        
        if (course == null)
            throw new KeyNotFoundException($"Course with id {request.CourseId} not found");
        
        dbContext.Courses.Remove(course);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}