using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Course;
using YTH_backend.Features.Courses.Commands;

namespace YTH_backend.Features.Courses.Handlers;

public class PatchCourseHandler(AppDbContext context) : IRequestHandler<PatchCourseCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(PatchCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await dbContext.Courses.FindAsync(request.CourseId, cancellationToken);
        
        if (course == null)
            throw new KeyNotFoundException($"Course with id {request.CourseId} not found");
        
        var dto = new PatchCourseRequestDto(course.Name, course.Description, course.Link);
        
        request.Patch.ApplyTo(dto);
        
        if (dto.Name is not null)
            course.Name = dto.Name;
        if (dto.Description is not null)
            course.Description = dto.Description;
        if (dto.Link is not null)
            course.Link = dto.Link;
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
