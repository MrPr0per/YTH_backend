using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.User;
using YTH_backend.Features.Users.Commands;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Features.Users.Handlers;

public class PatchUserHandler(AppDbContext dbContext) : IRequestHandler<PatchUserCommand>
{
    public async Task Handle(PatchUserCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FindAsync([request.Id], cancellationToken);
        
        if (user == null)
            throw new EntityNotFoundException($"User with id {request.Id} not found");

        var dto = new PatchUserRequestDto(user.UserName);
        
        request.PatchDocument.ApplyTo(dto);
        
        if (dto.UserName is not null)
            user.UserName = dto.UserName;
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}