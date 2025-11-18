using MediatR;
using YTH_backend.DTOs.Course;

namespace YTH_backend.Features.Courses.Commands;

public record AddCourseCommand(string Name, string Description, string Link) : IRequest<AddCourseResponseDto>;