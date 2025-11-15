namespace YTH_backend.Infrastructure.Exceptions;

public class TokenExpiredException(string message) : Exception(message);
