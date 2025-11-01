using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.Course;
using YTH_backend.Enums;
using YTH_backend.Features.Courses.Queries;
using YTH_backend.Models;
using YTH_backend.Models.Course;

namespace YTH_backend.Features.Courses.Handlers;

public class GetAllCoursesHandler(AppDbContext context) : IRequestHandler<GetAllCoursesQuery, PagedResult<GetCourseResponseDto>>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task<PagedResult<GetCourseResponseDto>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Course> query = dbContext.Courses;
        
        query = request.OrderType == OrderType.Asc 
            ? query.OrderBy(x => x.Name)
            : query.OrderByDescending(x => x.Name);
        
        var data = await query
            .Skip(request.From - 1)
            .Take(request.Take)
            .Select(c => new GetCourseResponseDto(
                c.Name,
                c.Description,
                c.Link,
                c.CreatedAt))
            .ToListAsync(cancellationToken);
        
        return new PagedResult<GetCourseResponseDto>(
            request.From,
            request.Take,
            request.OrderType,
            data);
    }
}