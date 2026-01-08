using Microsoft.Extensions.Configuration;

namespace EPS.Extensions.RedisConfig;

/// <summary>
/// Extension methods for adding Redis configuration provider to <see cref="IConfigurationBuilder"/>.
/// </summary>
public static class RedisConfigurationBuilderExtensions
{
    /// <summary>
    /// Adds a Redis configuration source to the <see cref="IConfigurationBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="connectionString">The Redis connection string.</param>
    /// <param name="configurationKey">The Redis key containing the configuration JSON. Default is "app:configuration".</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddRedisConfiguration(
        this IConfigurationBuilder builder,
        string connectionString,
        string configurationKey = "app:configuration")
    {
        return builder.AddRedisConfiguration(options =>
        {
            options.ConnectionString = connectionString;
            options.ConfigurationKey = configurationKey;
        });
    }

    /// <summary>
    /// Adds a Redis configuration source to the <see cref="IConfigurationBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="connectionString">The Redis connection string.</param>
    /// <param name="configurationKey">The Redis key containing the configuration JSON.</param>
    /// <param name="optional">Whether the Redis configuration source is optional.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddRedisConfiguration(
        this IConfigurationBuilder builder,
        string connectionString,
        string configurationKey,
        bool optional)
    {
        return builder.AddRedisConfiguration(options =>
        {
            options.ConnectionString = connectionString;
            options.ConfigurationKey = configurationKey;
            options.Optional = optional;
        });
    }

    /// <summary>
    /// Adds a Redis configuration source to the <see cref="IConfigurationBuilder"/> with polling-based reload.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="connectionString">The Redis connection string.</param>
    /// <param name="configurationKey">The Redis key containing the configuration JSON.</param>
    /// <param name="pollingInterval">The interval at which to poll for configuration changes.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddRedisConfigurationWithPolling(
        this IConfigurationBuilder builder,
        string connectionString,
        string configurationKey,
        TimeSpan pollingInterval)
    {
        return builder.AddRedisConfiguration(options =>
        {
            options.ConnectionString = connectionString;
            options.ConfigurationKey = configurationKey;
            options.ReloadMode = ReloadMode.Polling;
            options.PollingInterval = pollingInterval;
        });
    }

    /// <summary>
    /// Adds a Redis configuration source to the <see cref="IConfigurationBuilder"/> with Pub/Sub-based reload.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="connectionString">The Redis connection string.</param>
    /// <param name="configurationKey">The Redis key containing the configuration JSON.</param>
    /// <param name="pubSubChannel">The Redis Pub/Sub channel for change notifications. Default is "app:configuration:changed".</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddRedisConfigurationWithPubSub(
        this IConfigurationBuilder builder,
        string connectionString,
        string configurationKey,
        string pubSubChannel = "app:configuration:changed")
    {
        return builder.AddRedisConfiguration(options =>
        {
            options.ConnectionString = connectionString;
            options.ConfigurationKey = configurationKey;
            options.ReloadMode = ReloadMode.PubSub;
            options.PubSubChannel = pubSubChannel;
        });
    }

    /// <summary>
    /// Adds a Redis configuration source to the <see cref="IConfigurationBuilder"/> with full configuration options.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="configureOptions">A delegate to configure the <see cref="RedisConfigurationOptions"/>.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddRedisConfiguration(
        this IConfigurationBuilder builder,
        Action<RedisConfigurationOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configureOptions);

        var options = new RedisConfigurationOptions();
        configureOptions(options);

        return builder.Add(new RedisConfigurationSource { Options = options });
    }
}
