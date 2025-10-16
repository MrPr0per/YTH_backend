using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Events.Commands;

namespace YTH_backend.Features.Events.Handlers;

public class AddEventHandler(AppDbContext context) : IRequestHandler<AddEventCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(AddEventCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}