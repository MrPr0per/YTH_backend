using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Post;
using YTH_backend.Enums;
using YTH_backend.Features.Posts.Queries;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Features.Posts.Handlers;

public class GetPostHandler(AppDbContext dbContext) : IRequestHandler<GetPostByIdQuery, GetPostResponseDto>
{
    public async Task<GetPostResponseDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await dbContext.Posts.FindAsync([request.PostId], cancellationToken);

        if (post == null)
            throw new EntityNotFoundException($"Post with id:{request.PostId} not found");

        if (post.PostStatus == PostStatus.Hidden && (!request.IsAdmin || request.CurrentUserId != post.AuthorId))
            throw new UnauthorizedAccessException();
        
        return new GetPostResponseDto(post.Id, post.AuthorId, post.Title, post.Description, post.PostStatus, post.CreatedAt);
    }
}