using MediatR;

namespace YTH_backend.Features.ExpertApplication.Commands;

public record SendExpertApplicationCommand(Guid ApplicationId, Guid UserId) : IRequest;