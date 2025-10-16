using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using YTH_backend.DTOs.Course;

namespace YTH_backend.Features.Events.Commands;

public record PatchCourseCommand(Guid CourseId, JsonPatchDocument<PatchCourseRequestDto> Patch) : IRequest;