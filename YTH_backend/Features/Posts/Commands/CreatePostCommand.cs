using MediatR;
using YTH_backend.Enums;

namespace YTH_backend.Features.Posts.Commands;

public record CreatePostCommand(Guid AuthorId, string Title, string ShortDescription, string Description, Status Status) : IRequest;