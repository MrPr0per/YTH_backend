using MediatR;
using YTH_backend.DTOs.Course;
using YTH_backend.Enums;
using YTH_backend.Models;
using YTH_backend.Models.Course;

namespace YTH_backend.Features.Courses.Queries;

public record GetAllCoursesQuery(int From, int Take, OrderType OrderType) : IRequest<PagedResult<GetCourseResponseDto>>;