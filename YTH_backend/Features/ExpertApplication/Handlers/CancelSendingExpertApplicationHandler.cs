using MediatR;
using YTH_backend.Data;
using YTH_backend.Enums;
using YTH_backend.Features.ExpertApplication.Commands;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models.ExpertApplication;

namespace YTH_backend.Features.ExpertApplication.Handlers;

public class CancelSendingExpertApplicationHandler(AppDbContext dbContext) : IRequestHandler<CancelSendingExpertApplicationCommand>
{
    public async Task Handle(CancelSendingExpertApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await dbContext.ExpertApplications.FindAsync([request.ExpertApplicationId], cancellationToken);

        if (application == null)
            throw new EntityNotFoundException($"Expert application with id: {request.ExpertApplicationId} does not exist");
        
        if (application.UserId != request.CurrentUserId)
            throw new UnauthorizedAccessException("User does not have permission to cancel sending other admins applications");

        if (application.Status != ExpertApplicationStatus.Sent)
            throw new InvalidOperationException("Expert application is not sent");

        application.Status = ExpertApplicationStatus.Created;
        var expertApplicationAction = new ExpertApplicationAction
        {
            Id = Guid.NewGuid(),
            ExpertApplicationId = application.Id,
            CreatedAt = DateTime.UtcNow,
            ExpertApplicationActionType = ExpertApplicationActionType.CanceledSending,
            Other = null
        };
        
        await dbContext.ExpertApplicationActions.AddAsync(expertApplicationAction, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}