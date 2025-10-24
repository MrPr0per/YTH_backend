using MediatR;

namespace YTH_backend.Features.Users.Commands;

//TODO разобраться с Jwt
public record ResetPasswordCommand(Guid UserId, string Jwt, string NewPassword) : IRequest;