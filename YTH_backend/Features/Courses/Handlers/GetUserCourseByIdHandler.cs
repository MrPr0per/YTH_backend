using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Course;
using YTH_backend.Features.Courses.Queries;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Features.Courses.Handlers;

public class GetUserCourseByIdHandler(AppDbContext dbContext) : IRequestHandler<GetUserCourseByIdQuery, GetUserCourseByIdResponseDto>
{
    public async Task<GetUserCourseByIdResponseDto> Handle(GetUserCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var registration =
            await dbContext.UserCourseRegistrations.FindAsync([request.RegistrationId], cancellationToken);
        
        if (registration == null)
            throw new EntityNotFoundException($"Course registration with id:{request.RegistrationId} was not found");
        
        if (registration.UserId != request.CurrentUserId)
            throw new UnauthorizedAccessException("You do not have permission to see other user's course");

        return new GetUserCourseByIdResponseDto(registration.Id, registration.CreatedAt, registration.UserId,
            registration.CourseId);
    }
}