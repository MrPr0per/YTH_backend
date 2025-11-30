using MediatR;

namespace YTH_backend.Features.ExpertApplication.Commands;

public record CancelAcceptanceExpertApplicationCommand(Guid ApplicationId, Guid CurrentUserId) : IRequest;