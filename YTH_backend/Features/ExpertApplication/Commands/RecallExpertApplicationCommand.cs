using MediatR;

namespace YTH_backend.Features.ExpertApplication.Commands;

public record RecallExpertApplicationCommand(Guid ApplicationId, Guid UserId) : IRequest;