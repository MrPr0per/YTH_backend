using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.Course;
using YTH_backend.Enums;
using YTH_backend.Features.Courses.Queries;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Features.Courses.Handlers;

public class GetUserAllCoursesHandler(AppDbContext dbContext) : IRequestHandler<GetUserCoursesQuery, PagedResult<GetUserCourseByIdResponseDto>>
{
    public async Task<PagedResult<GetUserCourseByIdResponseDto>> Handle(GetUserCoursesQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId != null)
        {
            var userExists = await dbContext.Users
                .AnyAsync(u => u.Id == request.UserId, cancellationToken);

            if (!userExists)
                throw new EntityNotFoundException($"User with id: {request.UserId} not found");

            if (request.UserId != request.CurrentUserId)
                throw new UnauthorizedAccessException("You can't read other users registrations");
        }
        else
        {
            if (!request.IsAdmin)
                throw new UnauthorizedAccessException("You can't read other users");
        }

        var coursesQuery = dbContext.UserCourseRegistrations.AsQueryable();
            
        if (request.CourseId != null) 
            coursesQuery = coursesQuery.Where(c => c.CourseId == request.CourseId);
        
        if (request.UserId != null)
            coursesQuery = coursesQuery.Where(c => c.UserId == request.UserId);

        var take = request.Take;
        
        if (take < 0)
            take = 10;

        coursesQuery = coursesQuery
            .ApplyOrderSettings(request.OrderType, request.OrderFieldName)
            .ApplyCursorSettings(request.CursorType, take, request.CursorId);
        
        var data = coursesQuery
            .Select(c => new GetUserCourseByIdResponseDto(
                c.Id,
                c.CreatedAt,
                c.UserId,
                c.CourseId))
            .ToList();
        
        return new PagedResult<GetUserCourseByIdResponseDto>(
            take,
            request.OrderFieldName,
            request.OrderType,
            request.CursorType,
            data);
    }
}