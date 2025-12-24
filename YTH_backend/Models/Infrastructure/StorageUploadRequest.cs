namespace YTH_backend.Models.Infrastructure;

public record StorageUploadRequest(string? Base64Content, string FileName, string ContentType, string Directory, bool IsPublic);