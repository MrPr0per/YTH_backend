using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Infrastructure.Object_storage;

public class YandexStorageService : IStorageService
{
    private readonly IAmazonS3 s3;
    private readonly string bucket;
    private readonly string endpoint;

    public YandexStorageService(
        string accessKey,
        string secretKey,
        string bucket,
        string endpoint = "https://storage.yandexcloud.net")
    {
        this.bucket = bucket;
        this.endpoint = endpoint;

        var credentials = new BasicAWSCredentials(accessKey, secretKey);

        var config = new AmazonS3Config
        {
            ServiceURL = endpoint,
            ForcePathStyle = true,
        };

        s3 = new AmazonS3Client(credentials, config);
    }
    
    public async Task<string> UploadAsync(StorageUploadRequest request, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);
        
        var cleanBase64 = StripBase64Prefix(request.Base64Content!);
        var bytes = Convert.FromBase64String(cleanBase64);

        await using var stream = new MemoryStream(bytes);
        
        var key = $"{request.Directory}/{request.FileName}";

        var awsRequest = new PutObjectRequest
        {
            BucketName = bucket,
            Key = key,
            InputStream = stream,
            ContentType = request.ContentType,
            CannedACL = request.IsPublic ? S3CannedACL.PublicRead : S3CannedACL.Private,
        };
        
        await s3.PutObjectAsync(awsRequest, cancellationToken);

        return $"{endpoint}/{bucket}/{key}";
    }

    public async Task DeleteByUrlAsync(string url, CancellationToken cancellationToken = default)
    {
        var key = ExtractKeyFromUrl(url);

        var request = new DeleteObjectRequest
        {
            BucketName = bucket,
            Key = key
        };

        await s3.DeleteObjectAsync(request, cancellationToken);
    }
    
    private static string StripBase64Prefix(string base64)
    {
        var commaIndex = base64.IndexOf(',');

        return commaIndex >= 0
            ? base64[(commaIndex + 1)..]
            : base64;
    }
    
    private static void ValidateRequest(StorageUploadRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Base64Content))
            throw new ArgumentException("Base64Content must be provided");

        if (string.IsNullOrWhiteSpace(request.FileName))
            throw new ArgumentException("FileName is required");

        if (string.IsNullOrWhiteSpace(request.ContentType))
            throw new ArgumentException("ContentType is required");
    }
    
    private string ExtractKeyFromUrl(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            throw new ArgumentException("Invalid storage url");

        // /bucket/path/to/file.ext
        var segments = uri.AbsolutePath.TrimStart('/').Split('/', 2);

        if (segments.Length != 2)
            throw new ArgumentException("Invalid storage url format");

        var bucketFromUrl = segments[0];
        if (!string.Equals(bucketFromUrl, bucket, StringComparison.Ordinal))
            throw new InvalidOperationException("Bucket mismatch");

        return segments[1];
    }
}