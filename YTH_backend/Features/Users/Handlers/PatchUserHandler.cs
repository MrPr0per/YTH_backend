using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Users.Commands;

namespace YTH_backend.Features.Users.Handlers;

public class PatchUserHandler(AppDbContext context) : IRequestHandler<PatchUserCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public Task Handle(PatchUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}