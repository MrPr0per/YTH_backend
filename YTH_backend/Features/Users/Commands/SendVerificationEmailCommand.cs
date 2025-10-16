using MediatR;

namespace YTH_backend.Features.Users.Commands;

public record SendVerificationEmailCommand(string Email) : IRequest;