using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Course;
using YTH_backend.Features.Courses.Commands;
using YTH_backend.Models.Course;

namespace YTH_backend.Features.Courses.Handlers;

public class AddCourseHandler(AppDbContext dbContext) : IRequestHandler<AddCourseCommand, AddCourseResponseDto>
{
    public async Task<AddCourseResponseDto> Handle(AddCourseCommand request, CancellationToken cancellationToken)
    {
        var newCourse = new Course
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Link = request.Link
        };
        
        await dbContext.Courses.AddAsync(newCourse, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new AddCourseResponseDto(newCourse.Id, newCourse.Name, newCourse.Description, newCourse.Link, newCourse.CreatedAt);
    }
}