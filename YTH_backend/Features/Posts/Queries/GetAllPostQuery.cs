using MediatR;
using YTH_backend.DTOs.Post;
using YTH_backend.Enums;
using YTH_backend.Models;

namespace YTH_backend.Features.Posts.Queries;

public record GetAllPostQuery(int From, int Take, OrderType OrderType) : IRequest<PagedResult<GetPostResponseDto>>;