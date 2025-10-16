using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Users.Commands;

namespace YTH_backend.Features.Users.Handlers;

public class AuthUserHandler(AppDbContext context) : IRequestHandler<AuthUserCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(AuthUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}