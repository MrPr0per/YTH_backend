using MediatR;

namespace YTH_backend.Features.ExpertApplication.Commands;

public record AcceptForReviewExpertApplicationCommand(Guid Id) : IRequest;