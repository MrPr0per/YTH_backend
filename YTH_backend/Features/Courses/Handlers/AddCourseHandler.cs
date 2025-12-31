using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Course;
using YTH_backend.Features.Courses.Commands;
using YTH_backend.Infrastructure.Object_storage;
using YTH_backend.Models.Course;

namespace YTH_backend.Features.Courses.Handlers;

public class AddCourseHandler(AppDbContext dbContext, ImageAdder imageAdder) : IRequestHandler<AddCourseCommand, AddCourseResponseDto>
{
    public async Task<AddCourseResponseDto> Handle(AddCourseCommand request, CancellationToken cancellationToken)
    {
        var imageUrl = null as string;
        var courseId = Guid.NewGuid();
        
        if (request.ImageBase64 != null)
            imageUrl = await imageAdder.AddImageToObjectStorage(request.ImageBase64,
                $"course_{courseId}_{DateTime.UtcNow:yyyyMMddHHmmss}", true);
        
        var newCourse = new Course
        {
            Id = courseId,
            Name = request.Name,
            Description = request.Description,
            Link = request.Link,
            ImageUrl = imageUrl,
            Price = request.Price
        };
        
        await dbContext.Courses.AddAsync(newCourse, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new AddCourseResponseDto(newCourse.Id, newCourse.Name, newCourse.Description, newCourse.Link, newCourse.CreatedAt, newCourse.ImageUrl, newCourse.Price);
    }
}