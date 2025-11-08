using MediatR;
using YTH_backend.Data;
using YTH_backend.Enums;
using YTH_backend.Features.ExpertApplication.Commands;

namespace YTH_backend.Features.ExpertApplication.Handlers;

public class RecallExpertApplicationHandler(AppDbContext context) : IRequestHandler<RecallExpertApplicationCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(RecallExpertApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await dbContext.ExpertApplications.FindAsync(request.ApplicationId, cancellationToken);
        
        if (application == null)
            throw new KeyNotFoundException($"Expert application with id: {request.ApplicationId} does not exist");
        
        if (application.UserId != request.UserId)
            throw new UnauthorizedAccessException("User does not have permission to send other users notifications");

        if (application.Status != ExpertApplicationStatus.Sent)
            throw new InvalidOperationException("Expert application is on review or not sent");
        
        application.Status = ExpertApplicationStatus.NotSent;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}