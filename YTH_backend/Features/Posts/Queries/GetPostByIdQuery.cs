using MediatR;
using YTH_backend.DTOs.Post;

namespace YTH_backend.Features.Posts.Queries;

public record GetPostByIdQuery(Guid PostId, bool IsAdmin, Guid CurrentUserId) : IRequest<GetPostResponseDto>;