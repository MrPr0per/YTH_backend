using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.ExpertApplication.Commands;

namespace YTH_backend.Features.ExpertApplication.Handlers;

public class DeleteExpertApplicationHandler(AppDbContext context) : IRequestHandler<DeleteExpertApplicationCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(DeleteExpertApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await dbContext.ExpertApplications.FindAsync(request.ApplicationId, cancellationToken);
        
        if (application == null)
            throw new KeyNotFoundException($"Expert application with id: {request.ApplicationId} does not exist");
        
        if (application.UserId != request.UserId)
            throw new UnauthorizedAccessException("User does not have permission to delete other users notifications");

        dbContext.ExpertApplications.Remove(application);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}