using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Posts.Commands;

namespace YTH_backend.Features.Posts.Handlers;

public class DeletePostHandler(AppDbContext context) : IRequestHandler<DeletePostCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await dbContext.Posts.FindAsync(request.PostId, cancellationToken);
        
        if (post != null)
        { 
            dbContext.Posts.Remove(post);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        throw new KeyNotFoundException($"Post with id:{request.PostId} not found");
    }
}