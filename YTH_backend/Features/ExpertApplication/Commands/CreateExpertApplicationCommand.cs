using MediatR;

namespace YTH_backend.Features.ExpertApplication.Commands;

public record CreateExpertApplicationCommand(Guid UserId, string Message) : IRequest;