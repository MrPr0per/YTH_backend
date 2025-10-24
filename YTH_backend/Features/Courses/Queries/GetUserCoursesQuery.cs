using MediatR;
using YTH_backend.DTOs.Course;
using YTH_backend.Enums;
using YTH_backend.Models;

namespace YTH_backend.Features.Courses.Queries;

public record GetUserCoursesQuery(Guid UserId, int From, int Take, OrderType OrderType) : IRequest<PagedResult<GetCourseResponseDto>>;