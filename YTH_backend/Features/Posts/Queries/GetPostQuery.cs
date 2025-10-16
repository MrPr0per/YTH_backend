using MediatR;
using YTH_backend.DTOs.Post;

namespace YTH_backend.Features.Posts.Queries;

public record GetPostQuery(Guid PostId) : IRequest<GetPostResponseDto>;