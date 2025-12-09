using MediatR;
using YTH_backend.Data;
using YTH_backend.Enums;
using YTH_backend.Features.ExpertApplication.Commands;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models.ExpertApplication;

namespace YTH_backend.Features.ExpertApplication.Handlers;

public class CancelAcceptanceExpertApplicationHandler(AppDbContext dbContext) : IRequestHandler<CancelAcceptanceExpertApplicationCommand>
{
    public async Task Handle(CancelAcceptanceExpertApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await dbContext.ExpertApplications.FindAsync([request.ApplicationId], cancellationToken);
        
        if (application == null)
            throw new EntityNotFoundException($"Expert application with id: {request.ApplicationId} does not exist");

        if (application.Status != ExpertApplicationStatus.AcceptedForReview)
            throw new InvalidOperationException("Expert application is not AcceptedForReview or reviewing by other admin");
        
        if (application.AcceptedBy != request.CurrentUserId)
            throw new UnauthorizedAccessException("User does not have permission to cancel other admins applications");
        
        application.Status = ExpertApplicationStatus.Sent;
        application.AcceptedBy = null;

        var expertApplicationAction = new ExpertApplicationAction
        {
            Id = Guid.NewGuid(),
            ExpertApplicationId = application.Id,
            CreatedAt = DateTime.UtcNow,
            ExpertApplicationActionType = ExpertApplicationActionType.CancelledAcceptance,
            Other = null
        };
        
        await dbContext.ExpertApplicationActions.AddAsync(expertApplicationAction, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}