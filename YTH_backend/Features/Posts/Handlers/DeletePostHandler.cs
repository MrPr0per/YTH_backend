using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Posts.Commands;

namespace YTH_backend.Features.Posts.Handlers;

public class DeletePostHandler(AppDbContext context) : IRequestHandler<DeletePostCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}