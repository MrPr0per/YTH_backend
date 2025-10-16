using MediatR;
using YTH_backend.Enums;

namespace YTH_backend.Features.Posts.Commands;

public record CreatePostCommand(string Title, string Description, Status Status, DateTime CreatedAt) : IRequest;