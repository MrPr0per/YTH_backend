using MediatR;
using YTH_backend.DTOs.Post;
using Microsoft.AspNetCore.JsonPatch;

namespace YTH_backend.Features.Posts.Commands;

public record PatchPostCommand(Guid PostId, JsonPatchDocument<PatchPostRequestDto> Patch) : IRequest;