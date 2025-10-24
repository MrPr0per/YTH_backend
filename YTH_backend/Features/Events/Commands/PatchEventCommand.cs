using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using YTH_backend.DTOs.Course;
using YTH_backend.DTOs.Event;

namespace YTH_backend.Features.Events.Commands;

public record PatchEventCommand(Guid EventId, JsonPatchDocument<PatchEventRequestDto> Patch) : IRequest;