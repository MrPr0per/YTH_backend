using MediatR;
using YTH_backend.DTOs.Course;

namespace YTH_backend.Features.Courses.Queries;

public record GetCourseQuery(Guid CourseId) : IRequest<GetCoursesResponseDto>;