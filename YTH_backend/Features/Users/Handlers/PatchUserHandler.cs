using MediatR;
using Microsoft.EntityFrameworkCore;
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
        
        if (request.CurrentUserId != user.Id)
            throw new UnauthorizedAccessException();

        
        
        var dto = new PatchUserRequestDto(user.UserName);
        
        request.PatchDocument.ApplyTo(dto);
        
        if (dto.UserName is not null)
        {
            var userWithSameName = await dbContext.Users.FirstOrDefaultAsync(x => x.UserName == dto.UserName, cancellationToken);
            
            if (userWithSameName is not null)
                throw new EntityAlreadyExistsException("User with the same name already exists");
            
            user.UserName = dto.UserName;
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}