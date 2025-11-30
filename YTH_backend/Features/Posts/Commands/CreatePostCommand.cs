using MediatR;
using YTH_backend.DTOs.Post;
using YTH_backend.Enums;

namespace YTH_backend.Features.Posts.Commands;

public record CreatePostCommand(Guid AuthorId, string Title, string Description, PostStatus PostStatus) : IRequest<CreatePostResponseDto>;