using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Post;
using YTH_backend.Features.Posts.Queries;

namespace YTH_backend.Features.Posts.Handlers;

public class GetPostHandler(AppDbContext context) : IRequestHandler<GetPostQuery, GetPostResponseDto>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task<GetPostResponseDto> Handle(GetPostQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}