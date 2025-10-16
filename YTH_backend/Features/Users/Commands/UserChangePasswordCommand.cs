using MediatR;

namespace YTH_backend.Features.Users.Commands;

public record UserChangePasswordCommand(Guid UserId, string OldPassword, string NewPassword) : IRequest;