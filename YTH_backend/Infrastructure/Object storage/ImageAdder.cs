using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Infrastructure.Object_storage;

public class ImageAdder(IStorageService storageService)
{
    public async Task<string> AddImageToObjectStorage(string imageBase64, string fileNameWithoutExtension, bool isPublic)
    {
        var imageUrlType = UrlContentTypeHelper.TryGetContentTypeFromDataUri(imageBase64);
        
        if (imageUrlType == null)
        {
            var bytes = Convert.FromBase64String(imageBase64);
            imageUrlType = ImageTypeHelper.DetectImageContentType(bytes);
        }

        var extension = ImageTypeHelper.GetExtensionFromMimeType(imageUrlType);
        var fileName = $"{fileNameWithoutExtension}{extension}";
        
        var awsRequest = new StorageUploadRequest(imageBase64, fileName, imageUrlType, "images", isPublic);
        
        return await storageService.UploadAsync(awsRequest);
    }
}