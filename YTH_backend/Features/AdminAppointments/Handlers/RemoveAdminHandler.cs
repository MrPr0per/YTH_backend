using MediatR;
using YTH_backend.Data;
using YTH_backend.Enums;
using YTH_backend.Features.AdminAppointments.Commands;

namespace YTH_backend.Features.AdminAppointments.Handlers;

public class RemoveAdminHandler(AppDbContext dbContext) : IRequestHandler<RemoveAdminCommand>
{
    public async Task Handle(RemoveAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FindAsync([request.RevokedId], cancellationToken);
        
        if (user == null)
            throw new KeyNotFoundException($"User with id {request.RevokedId} not found");

        user.Role = Roles.Student;
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}