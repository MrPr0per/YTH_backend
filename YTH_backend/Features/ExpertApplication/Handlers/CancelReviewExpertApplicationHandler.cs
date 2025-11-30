using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.Enums;
using YTH_backend.Features.ExpertApplication.Commands;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models.ExpertApplication;
using YTH_backend.Models.User;

namespace YTH_backend.Features.ExpertApplication.Handlers;

public class CancelReviewExpertApplicationHandler(AppDbContext dbContext) : IRequestHandler<CancelReviewExpertApplicationCommand>
{
    public async Task Handle(CancelReviewExpertApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await dbContext.ExpertApplications.FindAsync([request.Id], cancellationToken);
        
        if (application == null)
            throw new EntityNotFoundException($"Expert application with id: {request.Id} does not exist");
        
        if (application.Status != ExpertApplicationStatus.Reviewed)
            throw new InvalidOperationException("Expert application is not reviewed");
        
        var newApplication = await dbContext.ExpertApplications.FirstOrDefaultAsync(x => x.UserId == application.UserId && x.Status != ExpertApplicationStatus.Reviewed,  cancellationToken);
        
        if (newApplication != null)
            throw new InvalidOperationException("User is already sent new application, you cannot cancel review");
        
        //TODO мб другой статус будет
        application.Status = ExpertApplicationStatus.AcceptedForReview;
        application.AcceptedBy = request.CurrentUserId;
        application.IsApproved = null;
        application.ResolutionMessage = null;
        
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = application.UserId,
            Title = "Решение по вашей заявке на экспертность было отменено",
            NotificationText = "Решение по вашей заявке на экспертность было отменено. Вы были сняты с должности эксперта."
        };
        
        await dbContext.Notifications.AddAsync(notification, cancellationToken);
        
        var expertApplicationAction = new ExpertApplicationAction
        {
            Id = Guid.NewGuid(),
            ExpertApplicationId = application.Id,
            CreatedAt = DateTime.UtcNow,
            ExpertApplicationActionType = ExpertApplicationActionType.CancelledReview,
            Other = JsonSerializer.Serialize(new AcceptedPayload(application.AcceptedBy!.Value), JsonPayloadConverter.Default.AcceptedPayload)
        };
        await dbContext.ExpertApplicationActions.AddAsync(expertApplicationAction, cancellationToken);
        
        var user = await dbContext.Users.FindAsync([application.UserId], cancellationToken);
            
        if (user == null)
            throw new KeyNotFoundException($"User with id: {application.UserId} does not exist");
            
        if (user.Role == Roles.Expert)
            user.Role = Roles.Student;
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}