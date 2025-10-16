using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Users.Commands;

namespace YTH_backend.Features.Users.Handlers;

public class ForgotPasswordHandler(AppDbContext context) : IRequestHandler<ForgotPasswordCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}