using MediatR;
using YTH_backend.DTOs.Course;
using YTH_backend.Enums;
using YTH_backend.Models;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Features.Courses.Queries;

public record GetUserCoursesQuery(Guid? UserId, int Take, OrderType OrderType, CursorType CursorType, Guid? CursorId, string OrderFieldName, Guid? CourseId, Guid CurrentUserId, bool IsAdmin) : IRequest<PagedResult<GetUserCourseByIdResponseDto>>;