namespace YTH_backend.Models.Infrastructure;

public record JwtSettings(string Secret, int ExpiryInMinutes = 10);