using MediatR;

namespace YTH_backend.Features.Courses.Commands;

public record DeleteCourseCommand(Guid CourseId) : IRequest;