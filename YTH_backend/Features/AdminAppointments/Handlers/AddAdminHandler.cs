using MediatR;
using YTH_backend.Data;
using YTH_backend.Enums;
using YTH_backend.Features.AdminAppointments.Commands;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models.User;

namespace YTH_backend.Features.AdminAppointments.Handlers;

public class AddAdminHandler(AppDbContext dbContext) : IRequestHandler<AddAdminCommand>
{
    public async Task Handle(AddAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FindAsync([request.UserId], cancellationToken);
        
        if (user == null)
            throw new EntityNotFoundException($"User with id {request.UserId} not found");
        
        if (user.Role == Roles.Admin || user.Role == Roles.SuperAdmin)
            throw new EntityAlreadyExistsException($"Admin with id {request.UserId} already exists");

        user.Role = Roles.Admin;
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}