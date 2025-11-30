using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Posts.Commands;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Features.Posts.Handlers;

public class DeletePostHandler(AppDbContext dbContext) : IRequestHandler<DeletePostCommand>
{
    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await dbContext.Posts.FindAsync([request.PostId], cancellationToken);
        
        if (post == null)
            throw new EntityNotFoundException($"Post with id:{request.PostId} not found");
        
        dbContext.Posts.Remove(post);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}