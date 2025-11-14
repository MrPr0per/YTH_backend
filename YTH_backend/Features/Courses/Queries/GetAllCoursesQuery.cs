using MediatR;
using YTH_backend.DTOs.Course;
using YTH_backend.Enums;
using YTH_backend.Models;
using YTH_backend.Models.Course;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Features.Courses.Queries;

public record GetAllCoursesQuery(int Take, OrderType OrderType, CursorType CursorType, string OrderFieldName, Guid? CursorId) : IRequest<PagedResult<GetCourseResponseDto>>;