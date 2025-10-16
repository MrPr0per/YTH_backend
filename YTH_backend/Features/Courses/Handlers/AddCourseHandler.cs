using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Courses.Commands;

namespace YTH_backend.Features.Courses.Handlers;

public class AddCourseHandler(AppDbContext context) : IRequestHandler<AddCourseCommand>
{
    private readonly AppDbContext dbContext = context;

    public async Task Handle(AddCourseCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}