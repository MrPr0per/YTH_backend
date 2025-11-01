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
        var user = await dbContext.Users
            .Include(u => u.Courses)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        
        if (user == null)
            throw new KeyNotFoundException($"User with id {request.UserId} not found");
        
        var query = request.OrderType == OrderType.Asc 
            ? user.Courses.OrderBy(x => x.Name)
            : user.Courses.OrderByDescending(x => x.Name);
        
        var data = query
            .Skip(request.From - 1)
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