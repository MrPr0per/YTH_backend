using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using YTH_backend.DTOs.Event;
using YTH_backend.DTOs.ExpertApplication;

namespace YTH_backend.Features.ExpertApplication.Commands;

public record PatchExpertApplicationCommand(Guid ApplicationId, Guid UserId, JsonPatchDocument<PatchExpertApplicationRequestDto> Patch) : IRequest;