using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.Post;
using YTH_backend.Enums;
using YTH_backend.Features.Posts.Queries;
using YTH_backend.Infrastructure;
using YTH_backend.Models;
using YTH_backend.Models.Infrastructure;
using YTH_backend.Models.Post;

namespace YTH_backend.Features.Posts.Handlers;

public class GetAllPostHandler(AppDbContext dbContext) :  IRequestHandler<GetAllPostQuery, PagedResult<GetPostResponseDto>>
{
    public async Task<PagedResult<GetPostResponseDto>> Handle(GetAllPostQuery request, CancellationToken cancellationToken)
    {
        var take = request.Take;

        if (take < 0)
            take = 10;
        
        var query = dbContext.Posts
            .ApplyOrderSettings(request.OrderType, request.OrderFieldName)
            .ApplyCursorSettings(request.CursorType, take, request.CursorId);
        
        var data = await query
            .Select(post => new GetPostResponseDto(
                post.AuthorId,
                post.Title,
                post.Description,
                post.PostStatus,
                post.CreatedAt
            ))
            .ToListAsync(cancellationToken);

        return new PagedResult<GetPostResponseDto>(
            take,
            request.OrderFieldName,
            request.OrderType,
            request.CursorType,
            data
        );
    }

}