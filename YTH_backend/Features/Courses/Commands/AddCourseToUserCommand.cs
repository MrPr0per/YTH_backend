using MediatR;

namespace YTH_backend.Features.Courses.Commands;

public record AddCourseToUserCommand(Guid UserId, Guid CourseId) : IRequest;