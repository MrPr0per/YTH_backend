using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using YTH_backend.DTOs.Event;

namespace YTH_backend.Features.Courses.Commands;

public record PatchCourseCommand(Guid EventId, JsonPatchDocument<PatchEventsRequestDto> Patch) : IRequest;