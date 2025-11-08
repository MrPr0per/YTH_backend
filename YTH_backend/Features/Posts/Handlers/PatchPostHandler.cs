using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Post;
using YTH_backend.Features.Posts.Commands;

namespace YTH_backend.Features.Posts.Handlers;

public class PatchPostHandler(AppDbContext context) : IRequestHandler<PatchPostCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(PatchPostCommand request, CancellationToken cancellationToken)
    {
        var id = request.PostId;
        var post = await dbContext.Posts.FindAsync(id, cancellationToken);
        
        if (post == null)
            throw new KeyNotFoundException($"Post with id:{id} not found");
        
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
    }
}