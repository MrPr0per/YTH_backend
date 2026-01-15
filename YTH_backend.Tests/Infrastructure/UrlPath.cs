namespace YTH_backend.Tests.Infrastructure;

public readonly struct UrlPath(string path = "")
{
    private readonly string path = path.Trim('/');

    public UrlPath(params string[] parts) : this(
        parts.Aggregate(new UrlPath(), (acc, part) => acc / part)
    )
    {
    }

    public static UrlPath operator /(UrlPath left, UrlPath right)
    {
        if (string.IsNullOrEmpty(left.path)) return right;
        if (string.IsNullOrEmpty(right.path)) return left;
        return new UrlPath($"{left.path}/{right.path}");
    }

    public static UrlPath operator /(UrlPath left, string right) =>
        left / new UrlPath(right);

    public static UrlPath operator /(string left, UrlPath right) =>
        new UrlPath(left) / right;

    public override string ToString()
        => "/" + path;

    public static implicit operator string(UrlPath path)
        => path.ToString();
}