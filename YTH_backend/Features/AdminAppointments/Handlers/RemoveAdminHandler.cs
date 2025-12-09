using MediatR;
using YTH_backend.Data;
using YTH_backend.Enums;
using YTH_backend.Features.AdminAppointments.Commands;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Features.AdminAppointments.Handlers;

public class RemoveAdminHandler(AppDbContext dbContext) : IRequestHandler<RemoveAdminCommand>
{
    public async Task Handle(RemoveAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FindAsync([request.RevokedId], cancellationToken);
        
        if (user == null)
            throw new EntityNotFoundException($"User with id {request.RevokedId} not found");

        if (request.RevokedId == request.CurrentUserId)
            throw new InvalidOperationException("You cannot revoke yourself");
            
        if (user.Role != Roles.Admin && user.Role != Roles.SuperAdmin)
            throw new EntityAlreadyExistsException($"User with id {request.RevokedId} is not admin already");
        
        user.Role = Roles.Student;
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}