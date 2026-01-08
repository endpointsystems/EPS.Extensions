namespace EPS.Extensions.RedisConfig;

/// <summary>
/// Specifies how the configuration provider should handle configuration changes.
/// </summary>
public enum ReloadMode
{
    /// <summary>
    /// Configuration is loaded once at startup and never reloaded.
    /// </summary>
    None,

    /// <summary>
    /// Configuration is periodically polled for changes.
    /// </summary>
    Polling,

    /// <summary>
    /// Configuration changes are detected via Redis Pub/Sub notifications.
    /// </summary>
    PubSub
}

/// <summary>
/// Options for configuring the Redis configuration provider.
/// </summary>
public class RedisConfigurationOptions
{
    /// <summary>
    /// Gets or sets the Redis connection string.
    /// Example: "localhost:6379" or "redis://user:password@host:port"
    /// </summary>
    public string ConnectionString { get; set; } = "localhost:6379";

    /// <summary>
    /// Gets or sets the Redis key where the configuration JSON document is stored.
    /// </summary>
    public string ConfigurationKey { get; set; } = "app:configuration";

    /// <summary>
    /// Gets or sets the reload mode for configuration changes.
    /// </summary>
    public ReloadMode ReloadMode { get; set; } = ReloadMode.None;

    /// <summary>
    /// Gets or sets the polling interval when <see cref="ReloadMode"/> is set to <see cref="ReloadMode.Polling"/>.
    /// Default is 30 seconds.
    /// </summary>
    public TimeSpan PollingInterval { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets or sets the Redis Pub/Sub channel name for configuration change notifications
    /// when <see cref="ReloadMode"/> is set to <see cref="ReloadMode.PubSub"/>.
    /// </summary>
    public string PubSubChannel { get; set; } = "app:configuration:changed";

    /// <summary>
    /// Gets or sets whether the configuration provider is optional.
    /// If true, the provider will not throw an exception if Redis is unavailable.
    /// </summary>
    public bool Optional { get; set; } = false;

    /// <summary>
    /// Gets or sets the timeout for Redis operations.
    /// </summary>
    public TimeSpan CommandTimeout { get; set; } = TimeSpan.FromSeconds(5);
}
