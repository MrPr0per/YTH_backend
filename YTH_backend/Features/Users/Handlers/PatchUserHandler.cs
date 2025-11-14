using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Users.Commands;

namespace YTH_backend.Features.Users.Handlers;

public class PatchUserHandler(AppDbContext dbContext) : IRequestHandler<PatchUserCommand>
{
    public Task Handle(PatchUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}