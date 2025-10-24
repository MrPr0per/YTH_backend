using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Courses.Commands;

namespace YTH_backend.Features.Courses.Handlers;

public class DeleteCourseFromUserHandler(AppDbContext context) : IRequestHandler<DeleteCourseFromUserCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(DeleteCourseFromUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}