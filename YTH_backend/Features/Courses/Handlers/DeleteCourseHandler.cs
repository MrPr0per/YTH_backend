using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Courses.Commands;
using YTH_backend.Features.Events.Commands;

namespace YTH_backend.Features.Courses.Handlers;

public class DeleteCourseHandler(AppDbContext context) : IRequestHandler<DeleteCourseCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public Task Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}