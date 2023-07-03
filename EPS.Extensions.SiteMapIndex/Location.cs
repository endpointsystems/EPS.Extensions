using System;

namespace EPS.Extensions.SiteMapIndex;

/// <summary>
/// A quick and dirty <see cref="Location"/> builder.
/// </summary>
public class LocationBuilder
{
    private static string basePath;
    public LocationBuilder(string baseUrl)
    {
        basePath = baseUrl;
        if (!basePath.EndsWith("/")) basePath += "/";
    }

    /// <summary>
    /// Build a <see cref="Location"/>.
    /// </summary>
    /// <param name="relativePath">The relative path</param>
    /// <param name="lastModified">The last modified date.</param>
    /// <param name="priority">The priority.</param>
    /// <param name="changeFrequency">The change frequency.</param>
    /// <returns>A new <see cref="Location"/>.</returns>
    public Location Build(string relativePath, DateTime lastModified, double priority, ChangeFrequency changeFrequency)
    {
        return build(relativePath, lastModified, priority, changeFrequency);
    }

    /// <summary>
    /// Build a <see cref="Location"/> with a default change frequency of Always.
    /// </summary>
    /// <param name="relativePath">The relative path</param>
    /// <param name="lastModified">The last modified date.</param>
    /// <param name="priority">The priority.</param>
    /// <returns>A new <see cref="Location"/>.</returns>
    public Location Build(string relativePath, DateTime lastModified, double priority)
    {
        return build(relativePath, lastModified, priority, ChangeFrequency.Always);
    }

    /// <summary>
    /// Build a <see cref="Location"/> with a default change frequency of Always
    /// and a priority of 1.0
    /// </summary>
    /// <param name="relativePath">The relative path</param>
    /// <param name="lastModified">The last modified date.</param>
    /// <returns>A new <see cref="Location"/>.</returns>
    public Location Build(string relativePath, DateTime lastModified)
    {
        return build(relativePath, lastModified, 1.0, ChangeFrequency.Always);
    }

    /// <summary>
    /// Build a <see cref="Location"/> with a default change frequency of Always,
    /// a priority of 1.0 and a modified date of the current time.
    /// </summary>
    /// <param name="relativePath">The relative path</param>
    /// <returns>A new <see cref="Location"/>.</returns>
    public Location Build(string relativePath)
    {
        return build(relativePath, DateTime.Now, 1.0, ChangeFrequency.Always);
    }

    private Location build(string relativePath, DateTime lastModified, double priority,
        ChangeFrequency changeFrequency)
    {
        if (relativePath.StartsWith("/"))
            relativePath = relativePath.Substring(1, relativePath.Length - 1);
            
        return new Location
        {
            Frequency = changeFrequency,
            Priority = priority,
            LastMod = lastModified,
            Url = basePath + relativePath
        };
    }
}
/// <summary>
/// The page entry for the site map.
/// </summary>
public class Location
{
    /// <summary>
    /// The full URL of the web page.
    /// </summary>
    public string Url { get; init; }
    /// <summary>
    /// Last modified date of the URL.
    /// </summary>
    public DateTime LastMod { get; init; }
    /// <summary>
    /// The frequency of the page updating.
    /// </summary>
    public ChangeFrequency Frequency { get; init; }
    /// <summary>
    /// Page priority.
    /// </summary>
    public double Priority { get; init; }

}

/// <summary>
/// The change frequency for the URL.
/// </summary>
public enum ChangeFrequency
{
    Always,
    Hourly,
    Daily,
    Weekly,
    Monthly,
    Yearly,
    Never
}