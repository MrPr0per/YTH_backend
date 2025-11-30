using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.ExpertApplication;
using YTH_backend.Enums;
using YTH_backend.Features.ExpertApplication.Commands;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Features.ExpertApplication.Handlers;

public class PatchExpertApplicationHandler(AppDbContext dbContext) : IRequestHandler<PatchExpertApplicationCommand, GetExpertApplicationResponseDto>
{
    public async Task<GetExpertApplicationResponseDto> Handle(PatchExpertApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await dbContext.ExpertApplications.FindAsync([request.ApplicationId], cancellationToken);
        
        if (application == null)
            throw new EntityNotFoundException($"Expert application with id: {request.ApplicationId} does not exist");
        
        if (application.Status >= ExpertApplicationStatus.AcceptedForReview)
            throw new InvalidOperationException($"Expert application with id: {request.ApplicationId} can't be patched");
        
        if (application.UserId != request.UserId)
            throw new UnauthorizedAccessException("User does not have permission to patch other users applications");

        var dto = new PatchExpertApplicationRequestDto(application.Message);
        
        request.Patch.ApplyTo(dto);
        
        if (dto.Message is not null)
            application.Message = dto.Message;
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return new GetExpertApplicationResponseDto(application.Id, application.UserId, application.Message,
            application.Status, application.AcceptedBy, application.IsApproved, application.ResolutionMessage);
    }
    
    
}