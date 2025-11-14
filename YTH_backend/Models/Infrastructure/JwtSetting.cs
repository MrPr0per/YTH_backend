namespace YTH_backend.Models.Infrastructure;

public record JwtSetting(string Secret, int ExpiryInMinutes = 10);