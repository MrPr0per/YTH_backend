using MediatR;
using Newtonsoft.Json;
using YTH_backend.Data;
using YTH_backend.Enums;
using YTH_backend.Features.ExpertApplication.Commands;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models.ExpertApplication;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace YTH_backend.Features.ExpertApplication.Handlers;

public class AcceptForReviewExpertApplicationHandler(AppDbContext dbContext) : IRequestHandler<AcceptForReviewExpertApplicationCommand>
{
    public async Task Handle(AcceptForReviewExpertApplicationCommand request, CancellationToken cancellationToken)
    {
        //TODO таймер
        var application = await dbContext.ExpertApplications.FindAsync([request.Id], cancellationToken);
        
        if (application == null)
            throw new EntityNotFoundException($"Expert application with id: {request.Id} does not exist");

        if (application.Status != ExpertApplicationStatus.Sent)
            throw new InvalidOperationException("Expert application is not sent or reviewing by other admin");
        
        application.Status = ExpertApplicationStatus.AcceptedForReview;
        application.AcceptedBy = request.CurrentUserId;
        
        var expertApplicationAction = new ExpertApplicationAction
        {
            Id = Guid.NewGuid(),
            ExpertApplicationId = application.Id,
            CreatedAt = DateTime.UtcNow,
            ExpertApplicationActionType = ExpertApplicationActionType.Accepted,
            Other = JsonSerializer.Serialize(new AcceptedPayload(request.CurrentUserId), JsonPayloadConverter.Default.AcceptedPayload)
        };
        
        await dbContext.ExpertApplicationActions.AddAsync(expertApplicationAction, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}