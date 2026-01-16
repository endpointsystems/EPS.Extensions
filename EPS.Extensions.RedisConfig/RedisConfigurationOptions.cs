namespace EPS.Extensions.RedisConfig;

/// <summary>
/// Specifies how the configuration is stored in Redis.
/// </summary>
public enum StorageMode
{
    /// <summary>
    /// Store configuration as a plain Redis string (default, no modules required).
    /// </summary>
    String,

    /// <summary>
    /// Store configuration using RedisJSON module (requires RedisJSON module installed on Redis server).
    /// </summary>
    RedisJson
}

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
    /// Gets or sets how the configuration is stored in Redis.
    /// Default is <see cref="StorageMode.String"/> which stores JSON as a plain string.
    /// Use <see cref="StorageMode.RedisJson"/> to use the RedisJSON module.
    /// </summary>
    public StorageMode StorageMode { get; set; } = StorageMode.String;

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
