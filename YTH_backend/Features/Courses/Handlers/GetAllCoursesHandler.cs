using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.Course;
using YTH_backend.Enums;
using YTH_backend.Features.Courses.Queries;
using YTH_backend.Infrastructure;
using YTH_backend.Models;
using YTH_backend.Models.Course;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Features.Courses.Handlers;

public class GetAllCoursesHandler(AppDbContext dbContext) : IRequestHandler<GetAllCoursesQuery, PagedResult<GetCourseResponseDto>>
{
    public async Task<PagedResult<GetCourseResponseDto>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Courses
            .ApplyOrderSettings(request.OrderType, request.OrderFieldName)
            .ApplyCursorSettings(request.CursorType, request.Take, request.CursorId, x => x.Id);
        
        var data = await query
            .Select(c => new GetCourseResponseDto(
                c.Name,
                c.Description,
                c.Link,
                c.CreatedAt))
            .ToListAsync(cancellationToken);
        
        return new PagedResult<GetCourseResponseDto>(
            request.Take,
            request.OrderFieldName,
            request.OrderType,
            request.CursorType,
            data);
    }
}