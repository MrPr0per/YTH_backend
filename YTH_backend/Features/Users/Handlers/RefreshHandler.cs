using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using YTH_backend.Data;
using YTH_backend.DTOs.User;
using YTH_backend.Enums;
using YTH_backend.Features.Users.Commands;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models.Infrastructure;
using YTH_backend.Models.User;

namespace YTH_backend.Features.Users.Handlers;

public class RefreshHandler(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, JwtSettings jwtSettings, IDistributedCache cache) : IRequestHandler<RefreshCommand, RefreshTokenResponseDto>
{
    private const int MaxRefreshAttempts = 5;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);
    public async Task<RefreshTokenResponseDto> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var context = httpContextAccessor.HttpContext
                      ?? throw new InvalidOperationException("HttpContext is null");
        
        var fp = FingerprintHelper.GetFingerprint(context);
        
        var cacheKey = $"refresh_attempt_fp:{fp}";
        
        var attemptsString = await cache.GetStringAsync(cacheKey, cancellationToken);
        //TODO проблема с гонкой
        var attempts = int.TryParse(attemptsString, out var n) ? n : 0;
        
        if (attempts >= MaxRefreshAttempts)
            throw new TooManyRequestsException("Too many refresh attempts. Try again later.");
        
        
        
        var oldRefreshToken = context.Request.Cookies["refreshToken"] 
                           ?? context.Request.Headers["Refresh-Token"].ToString();
        
        if (string.IsNullOrEmpty(oldRefreshToken))
            throw new TokenExpiredException("Refresh token is missing");
        
        var oldRefreshTokenHash = JwtHelper.HashRefreshToken(oldRefreshToken);
        
        var oldRefreshTokenEntity = await dbContext.RefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.TokenHash == oldRefreshTokenHash, cancellationToken);

        if (oldRefreshTokenEntity == null)
            await InvalidRefreshToken(cacheKey, attempts, cancellationToken);
        
        dbContext.RefreshTokens.Remove(oldRefreshTokenEntity!);
        
        if (oldRefreshTokenEntity!.ExpiresAt < DateTime.UtcNow)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            await InvalidRefreshToken(cacheKey, attempts, cancellationToken);
        }
        
        var user = oldRefreshTokenEntity.User;
        var accessToken = JwtHelper.GenerateVerificationToken(user, jwtSettings.Secret);
        var newRefreshToken = JwtHelper.GenerateRefreshToken();

        var refresh = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = JwtHelper.HashRefreshToken(newRefreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(30)
        };
        
        await dbContext.RefreshTokens.AddAsync(refresh, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new RefreshTokenResponseDto(accessToken, newRefreshToken);
    }

    private async Task InvalidRefreshToken(string cacheKey, int attempts, CancellationToken cancellationToken = default)
    {
        var newAttempts = attempts + 1;

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = LockoutDuration
        };
        
        await cache.SetStringAsync(cacheKey, newAttempts.ToString(), options, cancellationToken);
        var remaining = MaxRefreshAttempts - newAttempts;

        if (remaining > 0)
            throw new TokenExpiredException($"Refresh token is invalid or expired. {remaining} attempt(s) left.");
        
        throw new TooManyRequestsException("Too many failed attempts. Refresh token temporarily locked.");
    }
}