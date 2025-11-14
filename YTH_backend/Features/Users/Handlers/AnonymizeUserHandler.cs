using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Users.Commands;

namespace YTH_backend.Features.Users.Handlers;

public class AnonymizeUserHandler(AppDbContext dbContext) : IRequestHandler<AnonymizeUserCommand>
{
    public Task Handle(AnonymizeUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}