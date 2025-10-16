using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Post;
using YTH_backend.Features.Posts.Queries;
using YTH_backend.Models;

namespace YTH_backend.Features.Posts.Handlers;

public class GetAllPostHandler(AppDbContext context) :  IRequestHandler<GetAllPostQuery, PagedResult<GetPostResponseDto>>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task<PagedResult<GetPostResponseDto>> Handle(GetAllPostQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}