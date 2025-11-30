using MediatR;

namespace YTH_backend.Features.ExpertApplication.Commands;

public record CancelReviewExpertApplicationCommand(Guid Id) : IRequest;