using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using YTH_backend.Data;
using YTH_backend.Features.Users.Commands;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Features.Users.Handlers;

public class ChangePasswordHandler(AppDbContext dbContext) : IRequestHandler<ChangePasswordCommand>
{
    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FindAsync([request.UserId], cancellationToken);

        if (user == null)
            throw new EntityNotFoundException($"User with id: {request.UserId} not found");
        
        var oldHash = PasswordHasher.HashPassword(request.OldPassword, user.PasswordSalt);
        
        if (oldHash != user.PasswordHash)
            throw new UnauthorizedAccessException("Passwords do not match");
        
        var newSalt = PasswordHasher.GenerateSalt();
        var newHash = PasswordHasher.HashPassword(request.NewPassword, newSalt);

        user.PasswordHash = newHash;
        user.PasswordSalt = newSalt;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}