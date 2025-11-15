using MediatR;
using YTH_backend.Data;
using YTH_backend.Enums;
using YTH_backend.Features.ExpertApplication.Commands;
using YTH_backend.Models.User;

namespace YTH_backend.Features.ExpertApplication.Handlers;

public class CompleteReviewExpertApplicationHandler(AppDbContext dbContext) : IRequestHandler<CompleteReviewExpertApplicationCommand>
{
    public async Task Handle(CompleteReviewExpertApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await dbContext.ExpertApplications.FindAsync([request.ApplicationId], cancellationToken);
        var resolution = await dbContext.ExpertApplicationResolutions.FindAsync([request.ResolutionId], cancellationToken);
        
        if (resolution == null)
            throw new KeyNotFoundException($"Expert resolution with id: {request.ResolutionId} does not exist");
        
        if (application == null)
            throw new KeyNotFoundException($"Expert application with id: {request.ApplicationId} does not exist");

        if (resolution.DecidedById != request.AdminId)
            throw new UnauthorizedAccessException("User does not have permission to review other admin's reviews");
        
        if (application.Status != ExpertApplicationStatus.AcceptedForReview)
            throw new InvalidOperationException("Expert application is not reviewing");
        
        application.Status = ExpertApplicationStatus.Reviewed;

        var notification = new Notification()
        {
            Id = Guid.NewGuid(),
            UserId = application.UserId,
            Title = "Ваша заявка на экспертность была рассмотрена",
            NotificationText = resolution.IsApproved
                ? "Вы получили статус эксперта!"
                : "Вам было отказано в статусе эксперта"
        };

        await dbContext.Notifications.AddAsync(notification, cancellationToken);

        if (resolution.IsApproved)
        {
            var user = await dbContext.Users.FindAsync([application.UserId], cancellationToken);
            if (user == null)
                throw new KeyNotFoundException($"User with id: {application.UserId} does not exist");
            user.Role = Roles.Expert;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        
    }
}