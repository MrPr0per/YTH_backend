using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Course;
using YTH_backend.Features.Courses.Commands;
using YTH_backend.Features.Courses.Queries;

namespace YTH_backend.Features.Courses.Handlers;

public class GetCourseHandler(AppDbContext context) : IRequestHandler<GetCourseQuery, GetCourseResponseDto>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task<GetCourseResponseDto> Handle(GetCourseQuery request, CancellationToken cancellationToken)
    {
        var course = await dbContext.Courses.FindAsync(request.CourseId, cancellationToken);
        
        if (course == null)
            throw new KeyNotFoundException($"Course with id: {request.CourseId} not found");

        return new GetCourseResponseDto(
            course.Name,
            course.Description,
            course.Link,
            course.CreatedAt);
    }
}