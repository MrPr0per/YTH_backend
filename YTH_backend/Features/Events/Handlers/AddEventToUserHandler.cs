using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Events.Commands;

namespace YTH_backend.Features.Events.Handlers;

public class AddEventToUserHandler(AppDbContext context) : IRequestHandler<AddEventToUserCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(AddEventToUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}