using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.Course;
using YTH_backend.Enums;
using YTH_backend.Features.Courses.Queries;
using YTH_backend.Infrastructure;
using YTH_backend.Models;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Features.Courses.Handlers;

public class GetUserCoursesHandler(AppDbContext dbContext) : IRequestHandler<GetUserCoursesQuery, PagedResult<GetCourseResponseDto>>
{
    public async Task<PagedResult<GetCourseResponseDto>> Handle(GetUserCoursesQuery request, CancellationToken cancellationToken)
    {
        var userExists = await dbContext.Users
            .AnyAsync(u => u.Id == request.UserId, cancellationToken);

        if (!userExists)
            throw new KeyNotFoundException($"User with id: {request.UserId} not found");
        
        var coursesQuery = dbContext.UserCourseRegistrations
            .Where(r => r.UserId == request.UserId)
            .Select(r => r.Course)
            .AsQueryable();

        var take = request.Take;
        
        if (take < 0)
            take = 10;

        coursesQuery = coursesQuery
            .ApplyOrderSettings(request.OrderType, request.OrderFieldName)
            .ApplyCursorSettings(request.CursorType, take, request.CursorId);
        
        var data = coursesQuery
            .Select(c => new GetCourseResponseDto(
                c.Id,
                c.Name,
                c.Description,
                c.Link,
                c.CreatedAt))
            .ToList();
        
        return new PagedResult<GetCourseResponseDto>(
            take,
            request.OrderFieldName,
            request.OrderType,
            request.CursorType,
            data);
    }
}