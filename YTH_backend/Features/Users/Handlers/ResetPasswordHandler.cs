using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Users.Commands;

namespace YTH_backend.Features.Users.Handlers;

public class ResetPasswordHandler(AppDbContext context) : IRequestHandler<ResetPasswordCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}