using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.ExpertApplication;
using YTH_backend.Enums;
using YTH_backend.Features.ExpertApplication.Queries;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Features.ExpertApplication.Handlers;

public class GetExpertApplicationByIdHandler(AppDbContext dbContext) : IRequestHandler<GetExpertApplicationByIdQuery,  GetExpertApplicationResponseDto>
{
    public async Task<GetExpertApplicationResponseDto> Handle(GetExpertApplicationByIdQuery request, CancellationToken cancellationToken)
    {
        var application = await dbContext.ExpertApplications.FindAsync([request.ApplicationId], cancellationToken);

        if (application == null)
            throw new EntityNotFoundException($"Expert application with id: {request.ApplicationId} not found");

        if (!request.IsAdmin && application.UserId != request.CurrentUserId ||
            request.IsAdmin && application.Status == ExpertApplicationStatus.Created)
            throw new UnauthorizedAccessException();

        return new GetExpertApplicationResponseDto(application.Id, application.UserId, application.Message,
            application.Status, application.AcceptedBy, application.IsApproved, application.ResolutionMessage);
    }
}