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
        var take = request.Take;

        if (take < 0)
            take = 10;
        
        var query = dbContext.Courses
            .ApplyOrderSettings(request.OrderType, request.OrderFieldName)
            .ApplyCursorSettings(request.CursorType, take, request.CursorId);
        
        var data = await query
            .Select(c => new GetCourseResponseDto(
                c.Id,
                c.Name,
                c.Description,
                c.Link,
                c.CreatedAt))
            .ToListAsync(cancellationToken);
        
        return new PagedResult<GetCourseResponseDto>(
            take,
            request.OrderFieldName,
            request.OrderType,
            request.CursorType,
            data);
    }
}