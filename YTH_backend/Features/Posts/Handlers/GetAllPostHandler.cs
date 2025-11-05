using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.Post;
using YTH_backend.Enums;
using YTH_backend.Features.Posts.Queries;
using YTH_backend.Models;
using YTH_backend.Models.Post;

namespace YTH_backend.Features.Posts.Handlers;

public class GetAllPostHandler(AppDbContext context) :  IRequestHandler<GetAllPostQuery, PagedResult<GetPostResponseDto>>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task<PagedResult<GetPostResponseDto>> Handle(GetAllPostQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Post> query = dbContext.Posts;
        
        query = request.OrderType == OrderType.Asc
            ? query.OrderBy(post => post.CreatedAt)
            : query.OrderByDescending(post => post.CreatedAt);
        
        var data = await query
            .Skip(request.From - 1)
            .Take(request.Take)
            .Select(post => new GetPostResponseDto(
                post.AuthorId,
                post.Title,
                post.Description,
                post.Status,
                post.CreatedAt
            ))
            .ToListAsync(cancellationToken);

        return new PagedResult<GetPostResponseDto>(
            request.From,
            request.Take,
            request.OrderType,
            data
        );
    }

}