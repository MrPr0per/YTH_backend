using MediatR;
using YTH_backend.DTOs.Course;

namespace YTH_backend.Features.Courses.Commands;

public record AddCourseCommand(string Name, string Description, string Link, string? ImageBase64, decimal Price) : IRequest<AddCourseResponseDto>;