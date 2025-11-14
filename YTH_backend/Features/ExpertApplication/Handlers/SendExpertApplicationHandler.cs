using MediatR;
using YTH_backend.Data;
using YTH_backend.Enums;
using YTH_backend.Features.ExpertApplication.Commands;

namespace YTH_backend.Features.ExpertApplication.Handlers;

public class SendExpertApplicationHandler(AppDbContext dbContext) : IRequestHandler<SendExpertApplicationCommand>
{
    public async Task Handle(SendExpertApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await dbContext.ExpertApplications.FindAsync(request.ApplicationId, cancellationToken);
        
        if (application == null)
            throw new KeyNotFoundException($"Expert application with id: {request.ApplicationId} does not exist");
        
        if (application.UserId != request.UserId)
            throw new UnauthorizedAccessException("User does not have permission to send other users notifications");

        if (application.Status != ExpertApplicationStatus.NotSent)
            throw new InvalidOperationException("Expert application is already sent");
        
        application.Status = ExpertApplicationStatus.Sent;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}