using MediatR;

namespace YTH_backend.Features.ExpertApplication.Commands;

public record CompleteReviewExpertApplicationCommand(Guid ApplicationId, Guid ResolutionId, Guid AdminId) : IRequest;