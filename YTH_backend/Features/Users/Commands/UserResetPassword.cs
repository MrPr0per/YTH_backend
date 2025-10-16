using MediatR;

namespace YTH_backend.Features.Users.Commands;

public record UserResetPassword(Guid UserId, string Jwt, string NewPassword) : IRequest;