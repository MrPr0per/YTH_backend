using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Users.Commands;

namespace YTH_backend.Features.Users.Handlers;

public class CreateUserHandler(AppDbContext context) : IRequestHandler<CreateUserCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}