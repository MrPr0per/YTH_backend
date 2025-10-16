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
        throw new NotImplementedException();
    }
}