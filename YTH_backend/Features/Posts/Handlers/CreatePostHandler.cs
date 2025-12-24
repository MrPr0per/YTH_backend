using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Post;
using YTH_backend.Features.Posts.Commands;
using YTH_backend.Infrastructure.Object_storage;
using YTH_backend.Models.Infrastructure;
using YTH_backend.Models.Post;

namespace YTH_backend.Features.Posts.Handlers;

public class CreatePostHandler(AppDbContext dbContext, ImageAdder imageAdder) : IRequestHandler<CreatePostCommand, CreatePostResponseDto>
{
    public async Task<CreatePostResponseDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var imageUrl = null as string;
        var postId = Guid.NewGuid();
        
        if (request.ImageBase64 != null)
            imageUrl = await imageAdder.AddImageToObjectStorage(request.ImageBase64,
                $"post_{postId}_{DateTime.UtcNow:yyyyMMddHHmmss}", true);
        
        var newPost = new Post
        {
            Id = postId,
            Title = request.Title,
            Description = request.Description,
            AuthorId = request.AuthorId,
            PostStatus = request.PostStatus,
            ImageUrl = imageUrl
        };
        
        await dbContext.Posts.AddAsync(newPost, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreatePostResponseDto(newPost.Id, newPost.AuthorId, newPost.Title, newPost.Description,
            newPost.PostStatus, newPost.CreatedAt, newPost.ImageUrl);
    }
}