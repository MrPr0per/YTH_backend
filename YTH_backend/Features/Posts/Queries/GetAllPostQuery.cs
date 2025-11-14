using MediatR;
using YTH_backend.DTOs.Post;
using YTH_backend.Enums;
using YTH_backend.Models;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Features.Posts.Queries;

public record GetAllPostQuery(int Take, OrderType OrderType, CursorType CursorType, string OrderFieldName, Guid? CursorId) : IRequest<PagedResult<GetPostResponseDto>>;