using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Post;
using YTH_backend.Features.Posts.Commands;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Features.Posts.Handlers;

public class PatchPostHandler(AppDbContext dbContext) : IRequestHandler<PatchPostCommand, GetPostResponseDto>
{
    public async Task<GetPostResponseDto> Handle(PatchPostCommand request, CancellationToken cancellationToken)
    {
        var id = request.PostId;
        var post = await dbContext.Posts.FindAsync([id], cancellationToken);
        
        if (post == null)
            throw new EntityNotFoundException($"Post with id:{id} not found");
        
        if (request.CurrentUserId != post.AuthorId)
            throw new UnauthorizedAccessException();
        
        var dto = new PatchPostRequestDto(post.Title, post.Description, post.PostStatus, post.CreatedAt);
        
        request.Patch.ApplyTo(dto);
        
        if (dto.Title is not null)
            post.Title = dto.Title;
        if (dto.Description is not null)
            post.Description = dto.Description;
        if (dto.Status is not null)
            post.PostStatus = dto.Status.Value;
        if (dto.CreatedAt is not null)
            post.CreatedAt = dto.CreatedAt.Value;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new GetPostResponseDto(post.Id, post.AuthorId, post.Title, post.Description, post.PostStatus, post.CreatedAt);
    }
}