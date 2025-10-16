using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Posts.Commands;

namespace YTH_backend.Features.Posts.Handlers;

public class CreatePostHandler(AppDbContext context): IRequestHandler<CreatePostCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}