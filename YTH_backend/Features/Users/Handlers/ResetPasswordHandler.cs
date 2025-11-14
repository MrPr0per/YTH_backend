using MediatR;
using YTH_backend.Data;
using YTH_backend.Features.Users.Commands;
using YTH_backend.Infrastructure;

namespace YTH_backend.Features.Users.Handlers;

public class ResetPasswordHandler(AppDbContext dbContext) : IRequestHandler<ResetPasswordCommand>
{
    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        //TODO разобраться с Jwt
        var user = await dbContext.Users.FindAsync(request.UserId, cancellationToken);
        
        if (user == null)
            throw new KeyNotFoundException($"User with id {request.UserId} not found");
        
        var newSalt = PasswordHasher.GenerateSalt();
        var newPasswordHash = PasswordHasher.HashPassword(request.NewPassword, newSalt);
        
        user.PasswordHash = newPasswordHash;
        user.PasswordSalt = newSalt;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        throw new NotImplementedException();
    }
}