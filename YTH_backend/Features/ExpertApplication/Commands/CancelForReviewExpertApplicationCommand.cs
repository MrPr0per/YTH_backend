using MediatR;

namespace YTH_backend.Features.ExpertApplication.Commands;

public record CancelForReviewExpertApplicationCommand(Guid Id) : IRequest;