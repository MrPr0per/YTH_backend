using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Courses.Commands;

namespace YTH_backend.Features.Courses.Handlers;

public class PatchCourseHandler(AppDbContext context) : IRequestHandler<PatchCourseCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(PatchCourseCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
