using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using YTH_backend.DTOs.User;

namespace YTH_backend.Features.Users.Commands;

public record PatchUserCommand(Guid Id, JsonPatchDocument<PatchUserRequestDto> PatchDocument) : IRequest;