using MediatR;

namespace YTH_backend.Features.Debug;

public record GetTokenWithConfirmedEmailCommand(string Email) : IRequest<string>;