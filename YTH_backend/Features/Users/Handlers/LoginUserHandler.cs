using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using YTH_backend.Data;
using YTH_backend.DTOs.User;
using YTH_backend.Features.Users.Commands;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models.Infrastructure;
using YTH_backend.Models.User;

namespace YTH_backend.Features.Users.Handlers;

public class LoginUserHandler(AppDbContext dbContext, JwtSettings jwtSettings, IDistributedCache cache, IHttpContextAccessor httpContextAccessor) : IRequestHandler<LoginUserCommand, LoginUserResponseDto>
{
    private const int MaxLoginAttempts = 5;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);
    
    public async Task<LoginUserResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var context = httpContextAccessor.HttpContext
                      ?? throw new InvalidOperationException("HttpContext is null");
        
        var fp = FingerprintHelper.GetFingerprint(context);
        
        var cacheKey = $"login_attempt_fp:{fp}";
        
        var attemptsString = await cache.GetStringAsync(cacheKey, cancellationToken);
        //TODO проблема с гонкой
        var attempts = int.TryParse(attemptsString, out var n) ? n : 0;

        if (attempts >= MaxLoginAttempts)
            throw new TooManyRequestsException($"Too many login attempts for {request.Login}. Try again later.");
        
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == request.Login, cancellationToken);
        
        if (user == null)
            await InvalidLoginOrPasswordHandler(cacheKey, attempts, cancellationToken);
        
        var passwordHash = PasswordHasher.HashPassword(request.Password, user!.PasswordSalt);
        
        if (passwordHash != user.PasswordHash)
            await InvalidLoginOrPasswordHandler(cacheKey, attempts, cancellationToken);
        
        await cache.RemoveAsync(cacheKey, cancellationToken);
        
        var accessToken = JwtHelper.GenerateVerificationToken(user, jwtSettings.Secret);
        
        var refreshToken = JwtHelper.GenerateRefreshToken();

        var refresh = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = JwtHelper.HashRefreshToken(refreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(30)
        };
        
        await dbContext.RefreshTokens.AddAsync(refresh, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new LoginUserResponseDto(accessToken, refreshToken);
    }

    private async Task InvalidLoginOrPasswordHandler(string cacheKey, int attempts, CancellationToken cancellationToken = default)
    {
        var newAttempts = attempts + 1;

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = LockoutDuration
        };

        await cache.SetStringAsync(cacheKey, newAttempts.ToString(), options, cancellationToken);
        var remaining = MaxLoginAttempts - newAttempts;
        
        if (remaining > 0)
            throw new UnauthorizedAccessException($"Invalid login or password. {remaining} attempt(s) left.");
        
        throw new TooManyRequestsException("Too many failed attempts. Account temporarily locked.");
    }
}