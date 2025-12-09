using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using YTH_backend.DTOs.User;

namespace YTH_backend.Features.Users.Commands;

public record PatchUserCommand(Guid Id, Guid CurrentUserId, JsonPatchDocument<PatchUserRequestDto> PatchDocument) : IRequest;