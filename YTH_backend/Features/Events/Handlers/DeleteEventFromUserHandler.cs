using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Events.Commands;

namespace YTH_backend.Features.Events.Handlers;

public class DeleteEventFromUserHandler(AppDbContext context) : IRequestHandler<DeleteEventFromUserCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(DeleteEventFromUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}