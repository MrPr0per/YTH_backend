using MediatR;

namespace YTH_backend.Features.ExpertApplication.Commands;

public record ReviewExpertApplicationCommand(Guid ApplicationId, Guid CurrentUserId, bool IsApproved, string Message) : IRequest;