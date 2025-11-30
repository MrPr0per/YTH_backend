using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.Course;
using YTH_backend.Features.Courses.Commands;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models.Course;
using YTH_backend.Models.User;

namespace YTH_backend.Features.Courses.Handlers;

public class AddCourseToUserHandler(AppDbContext dbContext) : IRequestHandler<AddCourseToUserCommand, AddCourseToUserResponseDto>
{
    public async Task<AddCourseToUserResponseDto> Handle(AddCourseToUserCommand request, CancellationToken cancellationToken)
    {
        if (request.CurrentUserId != request.UserId)
            throw new UnauthorizedAccessException();
        
        var userExists = await dbContext.Users
            .AnyAsync(u => u.Id == request.UserId, cancellationToken);

        if (!userExists)
            throw new EntityNotFoundException($"User with id: {request.UserId} not found");
        
        var courseExists = await dbContext.Courses
            .AnyAsync(c => c.Id == request.CourseId, cancellationToken);

        if (!courseExists)
            throw new EntityNotFoundException($"Course with id: {request.CourseId} not found");
        
        var alreadyRegistered = await dbContext.UserCourseRegistrations
            .AnyAsync(r => r.UserId == request.UserId && r.CourseId == request.CourseId, cancellationToken);

        if (alreadyRegistered)
            throw new EntityAlreadyExistsException(
                $"Course with id: {request.CourseId} is already registered to user with id: {request.UserId}");
        
        var registration = new UserCourseRegistration
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            CourseId = request.CourseId,
        };

        await dbContext.UserCourseRegistrations.AddAsync(registration, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new AddCourseToUserResponseDto(registration.Id, registration.CreatedAt, registration.UserId, registration.CourseId);
    }
}