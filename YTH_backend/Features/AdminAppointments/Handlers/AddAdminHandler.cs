using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.AdminAppointments.Commands;
using YTH_backend.Models.User;

namespace YTH_backend.Features.AdminAppointments.Handlers;

public class AddAdminHandler(AppDbContext context) : IRequestHandler<AddAdminCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(AddAdminCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}