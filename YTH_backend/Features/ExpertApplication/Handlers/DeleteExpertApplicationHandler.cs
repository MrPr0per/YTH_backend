using MediatR;
using YTH_backend.Data;
using YTH_backend.Enums;
using YTH_backend.Features.ExpertApplication.Commands;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Features.ExpertApplication.Handlers;

public class DeleteExpertApplicationHandler(AppDbContext dbContext) : IRequestHandler<DeleteExpertApplicationCommand>
{
    public async Task Handle(DeleteExpertApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await dbContext.ExpertApplications.FindAsync([request.ApplicationId], cancellationToken);

        if (application == null)
            throw new EntityNotFoundException($"Expert application with id: {request.ApplicationId} does not exist");

        if (application.UserId != request.UserId)
            throw new UnauthorizedAccessException("User does not have permission to delete other users applications");

        if (application.Status >= ExpertApplicationStatus.AcceptedForReview)
            throw new InvalidOperationException("Expert application status is accepted for review");

        var applicationActions = dbContext.ExpertApplicationActions.Where(x => x.ExpertApplicationId == application.Id);

        dbContext.ExpertApplicationActions.RemoveRange(applicationActions);
        dbContext.ExpertApplications.Remove(application);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}