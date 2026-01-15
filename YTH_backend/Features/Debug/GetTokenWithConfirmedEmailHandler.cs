using MediatR;
using YTH_backend.Infrastructure;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Features.Debug;

public class GetTokenWithConfirmedEmailHandler(JwtSettings jwtSettings)
    : IRequestHandler<GetTokenWithConfirmedEmailCommand, string>
{
    public Task<string> Handle(GetTokenWithConfirmedEmailCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(
            JwtHelper.GenerateVerificationToken(
                new Dictionary<string, object> { ["email"] = request.Email, ["id"] = Guid.NewGuid() },
                jwtSettings.Secret
            )
        );
    }
}