using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.User;
using YTH_backend.Enums;
using YTH_backend.Features.Users.Commands;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models.Infrastructure;
using YTH_backend.Models.User;

namespace YTH_backend.Features.Users.Handlers;

public class CreateUserHandler(AppDbContext dbContext, JwtSettings jwtSettings) : IRequestHandler<CreateUserCommand, CreateUserResponseDto>
{
    public async Task<CreateUserResponseDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await dbContext.Users.AnyAsync(u => u.UserName == request.UserName, cancellationToken))
            throw new EntityAlreadyExistsException($"User with username: {request.UserName} is already registered");
        
        if (await dbContext.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
            throw new EntityAlreadyExistsException($"User with email: {request.Email} is already registered");
            
        var salt = PasswordHasher.GenerateSalt();
        var passwordHash = PasswordHasher.HashPassword(request.Password, salt);

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName,
            PasswordHash = passwordHash,
            PasswordSalt = salt,
            Role = Roles.Student,
            Email = request.Email
        };
        
        await dbContext.Users.AddAsync(newUser, cancellationToken);
        
        var contextData = new Dictionary<string, object>
        {
            ["email"] = newUser.Email,
            ["id"] = newUser.Id
        };

        var accessToken = JwtHelper.GenerateVerificationToken(
            contextData,
            jwtSettings.Secret,
            10,
            ["student", "logged_in"]
        );
        
        var refreshToken = JwtHelper.GenerateRefreshToken();

        var refresh = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = newUser.Id,
            TokenHash = JwtHelper.HashRefreshToken(refreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(30)
        };
        
        await dbContext.RefreshTokens.AddAsync(refresh, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new CreateUserResponseDto(accessToken, refreshToken);
    }
}