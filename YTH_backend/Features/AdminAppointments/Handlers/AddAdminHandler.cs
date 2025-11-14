using MediatR;
using YTH_backend.Data;
using YTH_backend.Enums;
using YTH_backend.Features.AdminAppointments.Commands;
using YTH_backend.Models.User;

namespace YTH_backend.Features.AdminAppointments.Handlers;

public class AddAdminHandler(AppDbContext dbContext) : IRequestHandler<AddAdminCommand>
{
    public async Task Handle(AddAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FindAsync(request.UserId, cancellationToken);
        
        if (user == null)
            throw new KeyNotFoundException($"User with id {request.UserId} not found");

        user.Role = Roles.Admin;
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}