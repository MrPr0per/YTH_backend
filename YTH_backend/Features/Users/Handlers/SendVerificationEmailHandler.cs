using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Users.Commands;

namespace YTH_backend.Features.Users.Handlers;

public class SendVerificationEmailHandler(AppDbContext context) : IRequestHandler<SendVerificationEmailCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(SendVerificationEmailCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}