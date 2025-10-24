using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Events.Commands;


namespace YTH_backend.Features.Events.Handlers;

public class DeleteEventHandler(AppDbContext context) : IRequestHandler<DeleteEventCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}