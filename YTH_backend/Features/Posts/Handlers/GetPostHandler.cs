using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Post;
using YTH_backend.Features.Posts.Queries;

namespace YTH_backend.Features.Posts.Handlers;

public class GetPostHandler(AppDbContext context) : IRequestHandler<GetPostByIdQuery, GetPostResponseDto>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task<GetPostResponseDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}