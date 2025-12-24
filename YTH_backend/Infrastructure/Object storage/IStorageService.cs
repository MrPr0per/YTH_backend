using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Infrastructure.Object_storage;

public interface IStorageService
{
    Task<string> UploadAsync(
        StorageUploadRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteByUrlAsync(
        string url,
        CancellationToken cancellationToken = default);
}