using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Course;
using YTH_backend.Features.Courses.Queries;
using YTH_backend.Models;

namespace YTH_backend.Features.Courses.Handlers;

public class GetUserCoursesHandler(AppDbContext context) : IRequestHandler<GetUserCoursesQuery, PagedResult<GetCourseResponseDto>>
{
    private readonly AppDbContext dbContext = context;
    
    public Task<PagedResult<GetCourseResponseDto>> Handle(GetUserCoursesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}