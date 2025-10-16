using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Users.Commands;

namespace YTH_backend.Features.Users.Handlers;

public class ChangePasswordHandler(AppDbContext context) : IRequestHandler<ChangePasswordCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}