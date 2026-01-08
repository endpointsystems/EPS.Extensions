# EPS.Extensions.RedisConfig

A .NET 10 library that extends `Microsoft.Extensions.Configuration` to read configuration from Redis cache using Redis.OM.

## Features

- **JSON Document Storage**: Store your entire configuration as a JSON document in Redis
- **Multiple Reload Modes**:
  - **None**: Configuration loaded once at startup
  - **Polling**: Periodically poll Redis for changes
  - **Pub/Sub**: Real-time configuration updates via Redis Pub/Sub
- **Optional Provider**: Gracefully handle Redis unavailability
- **Full Configuration Binding**: Works with all standard .NET configuration patterns

## Installation

```bash
dotnet add package EPS.Extensions.RedisConfig
```

## Usage

### Basic Usage

```csharp
var builder = new ConfigurationBuilder();

builder.AddRedisConfiguration("localhost:6379", "myapp:config");

var configuration = builder.Build();
var value = configuration["MySetting"];
```

### With Polling Reload

```csharp
builder.AddRedisConfigurationWithPolling(
    connectionString: "localhost:6379",
    configurationKey: "myapp:config",
    pollingInterval: TimeSpan.FromSeconds(30));
```

### With Pub/Sub Reload

```csharp
builder.AddRedisConfigurationWithPubSub(
    connectionString: "localhost:6379",
    configurationKey: "myapp:config",
    pubSubChannel: "myapp:config:changed");
```

### Full Options Configuration

```csharp
builder.AddRedisConfiguration(options =>
{
    options.ConnectionString = "localhost:6379";
    options.ConfigurationKey = "myapp:config";
    options.ReloadMode = ReloadMode.Polling;
    options.PollingInterval = TimeSpan.FromSeconds(15);
    options.Optional = true;
    options.CommandTimeout = TimeSpan.FromSeconds(5);
});
```

### Optional Provider

```csharp
builder.AddRedisConfiguration(
    connectionString: "localhost:6379",
    configurationKey: "myapp:config",
    optional: true);
```

## Redis Data Format

Store your configuration in Redis as a JSON document:

```bash
redis-cli SET myapp:config '{"Logging":{"LogLevel":{"Default":"Information"}},"ConnectionStrings":{"Database":"Server=..."}}'
```

The JSON is automatically flattened to configuration keys:
- `Logging:LogLevel:Default` = `"Information"`
- `ConnectionStrings:Database` = `"Server=..."`

## Pub/Sub Notifications

When using `ReloadMode.PubSub`, publish a message to the configured channel to trigger a reload:

```bash
redis-cli PUBLISH myapp:config:changed "reload"
```

## License

MIT License - see LICENSE file for details.
