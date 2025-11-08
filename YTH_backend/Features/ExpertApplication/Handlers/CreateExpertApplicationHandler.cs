using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.Enums;
using YTH_backend.Features.ExpertApplication.Commands;

namespace YTH_backend.Features.ExpertApplication.Handlers;

public class CreateExpertApplicationHandler(AppDbContext context) : IRequestHandler<CreateExpertApplicationCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(CreateExpertApplicationCommand request, CancellationToken cancellationToken)
    {
        var oldApplication = await dbContext.ExpertApplications
            .FirstOrDefaultAsync(a => a.UserId == request.UserId && a.Status == ExpertApplicationStatus.Sent, cancellationToken);
        
        if (oldApplication != null)
            throw new InvalidOperationException("Expert application already exists");
        
        var newExpertApplication = new Models.User.ExpertApplication
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Message = request.Message,
            Status = ExpertApplicationStatus.NotSent
        };
        
        await dbContext.ExpertApplications.AddAsync(newExpertApplication, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}