using System.Text.Json;
using MediatR;
using YTH_backend.Data;
using YTH_backend.Enums;
using YTH_backend.Features.ExpertApplication.Commands;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models.ExpertApplication;
using YTH_backend.Models.User;

namespace YTH_backend.Features.ExpertApplication.Handlers;

public class ReviewExpertApplicationHandler(AppDbContext dbContext) : IRequestHandler<ReviewExpertApplicationCommand>
{
    public async Task Handle(ReviewExpertApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await dbContext.ExpertApplications.FindAsync([request.ApplicationId], cancellationToken);
        
        if (application == null)
            throw new EntityNotFoundException($"Expert application with id: {request.ApplicationId} does not exist");
        
        if (application.Status != ExpertApplicationStatus.AcceptedForReview)
            throw new InvalidOperationException("Expert application is not reviewing");
        
        if (application.AcceptedBy != request.CurrentUserId)
            throw new UnauthorizedAccessException("User does not have permission to review other admins applications");
        
        application.Status = ExpertApplicationStatus.Reviewed;
        application.IsApproved = request.IsApproved;
        application.ResolutionMessage = request.Message;

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = application.UserId,
            Title = "Ваша заявка на экспертность была рассмотрена",
            NotificationText = request.IsApproved
                ? $"Вы получили статус эксперта! Комментарий администратора: {request.Message}"
                : $"Вам было отказано в статусе эксперта. Комментарий администратора: {request.Message}"
        };

        await dbContext.Notifications.AddAsync(notification, cancellationToken);
        
        var expertApplicationAction = new ExpertApplicationAction
        {
            Id = Guid.NewGuid(),
            ExpertApplicationId = application.Id,
            CreatedAt = DateTime.UtcNow,
            ExpertApplicationActionType = ExpertApplicationActionType.Reviewed,
            Other = JsonSerializer.Serialize(new ReviewedPayload(request.IsApproved, request.Message), JsonPayloadConverter.Default.ReviewedPayload)
        };
        
        await dbContext.ExpertApplicationActions.AddAsync(expertApplicationAction, cancellationToken);
        
        if (request.IsApproved)
        {
            var user = await dbContext.Users.FindAsync([application.UserId], cancellationToken);
            
            if (user == null)
                throw new KeyNotFoundException($"User with id: {application.UserId} does not exist");
            
            user.Role = Roles.Expert;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}