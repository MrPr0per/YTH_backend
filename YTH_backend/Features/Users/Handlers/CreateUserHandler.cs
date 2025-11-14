using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.Enums;
using YTH_backend.Features.Users.Commands;
using YTH_backend.Infrastructure;
using YTH_backend.Models.User;

namespace YTH_backend.Features.Users.Handlers;

public class CreateUserHandler(AppDbContext dbContext) : IRequestHandler<CreateUserCommand>
{
    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await dbContext.Users.AnyAsync(u => u.UserName == request.UserName, cancellationToken))
            throw new InvalidOperationException($"User with username: {request.UserName} is already registered");
        
        if (await dbContext.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
            throw new InvalidOperationException($"User with email: {request.Email} is already registered");
            
        var salt = PasswordHasher.GenerateSalt();
        var passwordHash = PasswordHasher.HashPassword(request.Password, salt);

        var newUser = new User
        {
            UserName = request.UserName,
            PasswordHash = passwordHash,
            PasswordSalt = salt,
            Role = Roles.Student,
            Email = request.Email
        };
        
        await dbContext.Users.AddAsync(newUser, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}