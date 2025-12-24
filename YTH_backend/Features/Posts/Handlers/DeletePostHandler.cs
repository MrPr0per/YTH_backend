using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Posts.Commands;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Infrastructure.Object_storage;

namespace YTH_backend.Features.Posts.Handlers;

public class DeletePostHandler(AppDbContext dbContext, IStorageService storageService) : IRequestHandler<DeletePostCommand>
{
    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await dbContext.Posts.FindAsync([request.PostId], cancellationToken);
        
        if (post == null)
            throw new EntityNotFoundException($"Post with id:{request.PostId} not found");

        if (post.AuthorId != request.CurrentUserId)
            throw new UnauthorizedAccessException();
        
        if (post.ImageUrl != null)
            await storageService.DeleteByUrlAsync(post.ImageUrl, cancellationToken);
        
        dbContext.Posts.Remove(post);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}