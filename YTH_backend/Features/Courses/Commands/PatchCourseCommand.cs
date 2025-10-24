using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using YTH_backend.DTOs.Course;
using YTH_backend.DTOs.Event;

namespace YTH_backend.Features.Courses.Commands;

public record PatchCourseCommand(Guid CourseId, JsonPatchDocument<PatchCourseRequestDto> Patch) : IRequest;