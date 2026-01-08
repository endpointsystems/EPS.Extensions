using Microsoft.Extensions.Configuration;

namespace EPS.Extensions.RedisConfig;

/// <summary>
/// Represents a Redis configuration source that implements <see cref="IConfigurationSource"/>.
/// </summary>
public class RedisConfigurationSource : IConfigurationSource
{
    /// <summary>
    /// Gets or sets the options for the Redis configuration provider.
    /// </summary>
    public RedisConfigurationOptions Options { get; set; } = new();

    /// <summary>
    /// Builds the <see cref="IConfigurationProvider"/> for this source.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
    /// <returns>An <see cref="IConfigurationProvider"/>.</returns>
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new RedisConfigurationProvider(this);
    }
}
