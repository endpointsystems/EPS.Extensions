namespace EPS.Extensions.SiteMapIndex;

/// <summary>
/// Compliance settings for the site map indexes. 
/// </summary>
/// <remarks>
/// The maximum file size allowed is 50MB and
/// the maximum number of location entries is 50k.
/// </remarks>
public class SiteMapConfig
{
    public int MaxFileSize { get; init; } = 50000000;
    public int MaxLocationCount { get; init; } = 50000;
}