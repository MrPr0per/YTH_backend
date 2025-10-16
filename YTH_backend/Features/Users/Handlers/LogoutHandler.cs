using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Users.Commands;

namespace YTH_backend.Features.Users.Handlers;

public class LogoutHandler(AppDbContext context) : IRequestHandler<LogoutCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}