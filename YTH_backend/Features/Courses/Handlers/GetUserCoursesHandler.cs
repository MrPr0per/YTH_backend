using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.Course;
using YTH_backend.Enums;
using YTH_backend.Features.Courses.Queries;
using YTH_backend.Models;

namespace YTH_backend.Features.Courses.Handlers;

public class GetUserCoursesHandler(AppDbContext context) : IRequestHandler<GetUserCoursesQuery, PagedResult<GetCourseResponseDto>>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task<PagedResult<GetCourseResponseDto>> Handle(GetUserCoursesQuery request, CancellationToken cancellationToken)
    {
        var userExists = await dbContext.Users
            .AnyAsync(u => u.Id == request.UserId, cancellationToken);

        if (!userExists)
            throw new KeyNotFoundException($"User with id: {request.UserId} not found");
        
        var from = request.From < 1 ? 1 : request.From;
        var skip = from - 1;

        var coursesQuery = dbContext.UserCourseRegistrations
            .Where(r => r.UserId == request.UserId)
            .Select(r => r.Course)
            .AsQueryable();
        
        coursesQuery = request.OrderType == OrderType.Asc 
            ? coursesQuery.OrderBy(x => x.Name)
            : coursesQuery.OrderByDescending(x => x.Name);
        
        var data = coursesQuery
            .Skip(skip)
            .Take(request.Take)
            .Select(c => new GetCourseResponseDto(
                c.Name,
                c.Description,
                c.Link,
                c.CreatedAt))
            .ToList();
        
        return new PagedResult<GetCourseResponseDto>(
            request.From,
            request.Take,
            request.OrderType,
            data);
    }
}