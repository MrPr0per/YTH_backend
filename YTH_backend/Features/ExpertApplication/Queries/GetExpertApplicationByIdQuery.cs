using MediatR;
using YTH_backend.DTOs.ExpertApplication;

namespace YTH_backend.Features.ExpertApplication.Queries;

public record GetExpertApplicationByIdQuery(Guid ApplicationId, Guid CurrentUserId, bool IsAdmin) : IRequest<GetExpertApplicationResponseDto>;