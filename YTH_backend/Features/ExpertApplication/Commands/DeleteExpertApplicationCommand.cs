using MediatR;

namespace YTH_backend.Features.ExpertApplication.Commands;

public record DeleteExpertApplicationCommand(Guid UserId, Guid ApplicationId) : IRequest;