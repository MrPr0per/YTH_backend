using MediatR;

namespace YTH_backend.Features.Courses.Commands;

public record AddCourseCommand(string Name, string? Description, string? Link) : IRequest;