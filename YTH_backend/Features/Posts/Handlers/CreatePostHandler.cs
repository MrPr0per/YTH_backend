using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Post;
using YTH_backend.Features.Posts.Commands;
using YTH_backend.Models.Post;

namespace YTH_backend.Features.Posts.Handlers;

public class CreatePostHandler(AppDbContext dbContext) : IRequestHandler<CreatePostCommand, CreatePostResponseDto>
{
    public async Task<CreatePostResponseDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
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

        return new CreatePostResponseDto(newPost.Id, newPost.AuthorId, newPost.Title, newPost.Description,
            newPost.PostStatus, newPost.CreatedAt);
    }
}