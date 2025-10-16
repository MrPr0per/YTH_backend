using MediatR;

namespace YTH_backend.Features.Courses.Commands;

public record DeleteCourseFromUser(Guid UserId, Guid CourseId) : IRequest;