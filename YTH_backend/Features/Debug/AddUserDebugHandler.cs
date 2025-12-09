using MediatR;
using YTH_backend.Data;
using YTH_backend.Enums;
using YTH_backend.Infrastructure;
using YTH_backend.Models.Infrastructure;
using YTH_backend.Models.User;

namespace YTH_backend.Features.Debug;

public class AddUserDebugHandler(AppDbContext dbContext, JwtSettings jwtSettings) : IRequestHandler<AddUserDebugCommand, string>
{
    public async Task<string> Handle(AddUserDebugCommand request, CancellationToken cancellationToken)
    {
        var salt = PasswordHasher.GenerateSalt();
        var passwordHash = PasswordHasher.HashPassword(request.Password, salt);

        var role = request.Role switch
        {
            "admin" => Roles.Admin,
            "student" => Roles.Student,
            "superadmin" => Roles.SuperAdmin,
            "expert" => Roles.Expert,
            _ => Roles.Student
        };

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            UserName = request.Username,
            PasswordHash = passwordHash,
            PasswordSalt = salt,
            Role = role,
            Email = request.Email
        }; 
        
        await dbContext.Users.AddAsync(newUser, cancellationToken);

        var accessToken = JwtHelper.GenerateVerificationToken(newUser, jwtSettings.Secret);
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

        return accessToken;
    }
}