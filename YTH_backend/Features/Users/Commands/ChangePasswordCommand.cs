using MediatR;

namespace YTH_backend.Features.Users.Commands;

public record ChangePasswordCommand(Guid UserId, string OldPassword, string NewPassword) : IRequest;