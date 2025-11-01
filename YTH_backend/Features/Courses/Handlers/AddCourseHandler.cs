using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Courses.Commands;
using YTH_backend.Models.Course;

namespace YTH_backend.Features.Courses.Handlers;

public class AddCourseHandler(AppDbContext context) : IRequestHandler<AddCourseCommand>
{
    private readonly AppDbContext dbContext = context;

    public async Task Handle(AddCourseCommand request, CancellationToken cancellationToken)
    {
        var newCourse = new Course
        {
            Name = request.Name,
            Description = request.Description,
            Link = request.Link
        };
        
        await dbContext.Courses.AddAsync(newCourse, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}