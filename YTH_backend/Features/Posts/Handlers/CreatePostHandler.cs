using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Posts.Commands;
using YTH_backend.Models.Post;

namespace YTH_backend.Features.Posts.Handlers;

public class CreatePostHandler(AppDbContext context): IRequestHandler<CreatePostCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var newPost = new Post
        {
            Title = request.Title,
            Description = request.Description,
            AuthorId = request.AuthorId,
            PostStatus = request.PostStatus,
        };
        
        await dbContext.Posts.AddAsync(newPost, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}