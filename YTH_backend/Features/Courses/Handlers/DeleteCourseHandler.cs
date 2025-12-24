using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Courses.Commands;
using YTH_backend.Features.Events.Commands;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Infrastructure.Object_storage;

namespace YTH_backend.Features.Courses.Handlers;

public class DeleteCourseHandler(AppDbContext dbContext, IStorageService storageService) : IRequestHandler<DeleteCourseCommand>
{
    public async Task Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await dbContext.Courses.FindAsync([request.CourseId], cancellationToken);
        
        if (course == null)
            throw new EntityNotFoundException($"Course with id {request.CourseId} not found");
        
        if (course.ImageUrl != null)
            await storageService.DeleteByUrlAsync(course.ImageUrl, cancellationToken);
        
        dbContext.Courses.Remove(course);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}