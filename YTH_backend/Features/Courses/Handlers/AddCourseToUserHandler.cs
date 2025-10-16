using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Courses.Commands;

namespace YTH_backend.Features.Courses.Handlers;

public class AddCourseToUserHandler(AppDbContext context) : IRequestHandler<AddCourseToUserCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(AddCourseToUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}