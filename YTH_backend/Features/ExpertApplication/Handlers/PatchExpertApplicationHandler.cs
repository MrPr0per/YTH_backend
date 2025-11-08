using MediatR;
using YTH_backend.Data;
using YTH_backend.Enums;
using YTH_backend.Features.ExpertApplication.Commands;

namespace YTH_backend.Features.ExpertApplication.Handlers;

public class PatchExpertApplicationHandler(AppDbContext context) : IRequestHandler<PatchExpertApplicationCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(PatchExpertApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await dbContext.ExpertApplications.FindAsync(request.ApplicationId, cancellationToken);
        
        if (application == null)
            throw new KeyNotFoundException($"Expert application with id: {request.ApplicationId} does not exist");
        
        if (application.Status != ExpertApplicationStatus.Sent || application.Status != ExpertApplicationStatus.NotSent)
            throw new InvalidOperationException($"Expert application with id: {request.ApplicationId} can't be patched");
        
        if (application.UserId != request.UserId)
            throw new UnauthorizedAccessException("User does not have permission to patch other users applications");
    }
}