using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.ExpertApplication;
using YTH_backend.Enums;
using YTH_backend.Features.ExpertApplication.Commands;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models.ExpertApplication;

namespace YTH_backend.Features.ExpertApplication.Handlers;

public class CreateExpertApplicationHandler(AppDbContext dbContext) : IRequestHandler<CreateExpertApplicationCommand, CreateExpertApplicationResponseDto>
{
    public async Task<CreateExpertApplicationResponseDto> Handle(CreateExpertApplicationCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FindAsync([request.UserId], cancellationToken);
        
        if (user!.Role == Roles.Expert)
            throw new EntityAlreadyExistsException("User is already expert");
        
        var oldApplication = await dbContext.ExpertApplications
            .FirstOrDefaultAsync(a => a.UserId == request.UserId && a.Status != ExpertApplicationStatus.Reviewed, cancellationToken);
        
        if (oldApplication != null)
            throw new EntityAlreadyExistsException("Expert application already exists");
        
        var newExpertApplication = new Models.ExpertApplication.ExpertApplication
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Message = request.Message,
            Status = ExpertApplicationStatus.Created
        };

        var expertApplicationAction = new ExpertApplicationAction
        {
            Id =  Guid.NewGuid(),
            ExpertApplicationId = newExpertApplication.Id,
            CreatedAt = DateTime.UtcNow,
            ExpertApplicationActionType = ExpertApplicationActionType.Created,
            Other = null
        } ;
        
        await dbContext.ExpertApplicationActions.AddAsync(expertApplicationAction, cancellationToken);
        await dbContext.ExpertApplications.AddAsync(newExpertApplication, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateExpertApplicationResponseDto(newExpertApplication.Id, newExpertApplication.UserId,
            newExpertApplication.Message, newExpertApplication.Status);
    }
}