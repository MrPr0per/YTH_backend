using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Course;
using YTH_backend.Features.Courses.Queries;
using YTH_backend.Models;

namespace YTH_backend.Features.Courses.Handlers;

public class GetAllCoursesHandler(AppDbContext context) : IRequestHandler<GetAllCoursesQuery, PagedResult<GetCoursesResponseDto>>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task<PagedResult<GetCoursesResponseDto>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}