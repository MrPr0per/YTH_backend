using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.AdminAppointments.Commands;

namespace YTH_backend.Features.AdminAppointments.Handlers;

public class RemoveAdminHandler(AppDbContext context) : IRequestHandler<RemoveAdminCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(RemoveAdminCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}