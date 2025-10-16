using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Users.Commands;

namespace YTH_backend.Features.Users.Handlers;

public class RefreshHandler(AppDbContext context) : IRequestHandler<RefreshCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}