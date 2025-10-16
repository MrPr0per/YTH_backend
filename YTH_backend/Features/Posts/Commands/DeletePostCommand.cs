using MediatR;

namespace YTH_backend.Features.Posts.Commands;

public record DeletePostCommand(Guid PostId) : IRequest;