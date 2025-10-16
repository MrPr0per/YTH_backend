using MediatR;

namespace YTH_backend.Features.Users.Commands;

public record ForgotPasswordCommand(string Email) : IRequest;