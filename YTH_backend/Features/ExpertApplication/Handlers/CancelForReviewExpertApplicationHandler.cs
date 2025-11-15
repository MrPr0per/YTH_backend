using MediatR;
using YTH_backend.Data;
using YTH_backend.Enums;
using YTH_backend.Features.ExpertApplication.Commands;

namespace YTH_backend.Features.ExpertApplication.Handlers;

public class CancelForReviewExpertApplicationHandler(AppDbContext dbContext) : IRequestHandler<CancelForReviewExpertApplicationCommand>
{
    public async Task Handle(CancelForReviewExpertApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await dbContext.ExpertApplications.FindAsync([request.Id], cancellationToken);
        
        if (application == null)
            throw new KeyNotFoundException($"Expert application with id: {request.Id} does not exist");
        
        if (application.Status != ExpertApplicationStatus.AcceptedForReview)
            throw new InvalidOperationException("Expert application is not reviewing");
        
        application.Status = ExpertApplicationStatus.Sent;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}