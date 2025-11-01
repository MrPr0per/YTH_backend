using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Post;
using YTH_backend.Features.Posts.Queries;

namespace YTH_backend.Features.Posts.Handlers;

public class GetPostHandler(AppDbContext context) : IRequestHandler<GetPostByIdQuery, GetPostResponseDto?>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task<GetPostResponseDto?> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await dbContext.Posts.FindAsync(request.PostId, cancellationToken);

        if (post != null)
            return new GetPostResponseDto(post.AuthorId, post.Title, post.ShortDescription, post.Description, post.Status, post.CreatedAt);
        
        throw new KeyNotFoundException($"Post with id:{request.PostId} not found");
    }
}