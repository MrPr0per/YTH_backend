using MediatR;
using YTH_backend.Data;
using YTH_backend.Enums;
using YTH_backend.Features.ExpertApplication.Commands;

namespace YTH_backend.Features.ExpertApplication.Handlers;

public class AcceptForReviewExpertApplicationHandler(AppDbContext context) : IRequestHandler<AcceptForReviewExpertApplicationCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(AcceptForReviewExpertApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await dbContext.ExpertApplications.FindAsync(request.Id, cancellationToken);
        
        if (application == null)
            throw new KeyNotFoundException($"Expert application with id: {request.Id} does not exist");

        if (application.Status != ExpertApplicationStatus.Sent)
            throw new InvalidOperationException("Expert application is not sent or reviewing by other admin");
        
        application.Status = ExpertApplicationStatus.AcceptedForReview;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}