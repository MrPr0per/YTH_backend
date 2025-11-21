using MediatR;
using YTH_backend.DTOs.Course;

namespace YTH_backend.Features.Courses.Commands;

public record AddCourseToUserCommand(Guid UserId, Guid CourseId, Guid CurrentUserId) : IRequest<AddCourseToUserResponseDto>;