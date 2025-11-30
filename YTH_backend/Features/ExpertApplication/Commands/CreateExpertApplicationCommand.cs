using MediatR;
using YTH_backend.DTOs.ExpertApplication;

namespace YTH_backend.Features.ExpertApplication.Commands;

public record CreateExpertApplicationCommand(Guid UserId, string Message) : IRequest<CreateExpertApplicationResponseDto>;