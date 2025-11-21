using MediatR;

namespace YTH_backend.Features.Courses.Commands;

public record DeleteCourseFromUserCommand(Guid RegistrationId, Guid CurrentUserId) : IRequest;