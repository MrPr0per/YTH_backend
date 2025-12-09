using MediatR;

namespace YTH_backend.Features.Debug;

public record AddUserDebugCommand(string Username, string Password, string Email, string Role) : IRequest<string>;