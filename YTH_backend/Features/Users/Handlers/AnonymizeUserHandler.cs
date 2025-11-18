using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Users.Commands;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Features.Users.Handlers;

public class AnonymizeUserHandler(AppDbContext dbContext) : IRequestHandler<AnonymizeUserCommand>
{
    public async Task Handle(AnonymizeUserCommand request, CancellationToken cancellationToken)
    {
        if (request.CurrentUserId != request.UserId)
            throw new UnauthorizedAccessException();

        var user = await dbContext.Users.FindAsync([request.UserId], cancellationToken);

        if (user == null)
            throw new EntityNotFoundException($"User with id {request.UserId} not found");

        user.UserName = "";
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}