using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using Redis.OM;
using Redis.OM.Contracts;
using StackExchange.Redis;

namespace EPS.Extensions.RedisConfig;

/// <summary>
/// A configuration provider that reads configuration from Redis using Redis.OM.
/// Supports JSON document storage with optional polling or pub/sub change notification.
/// </summary>
[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
public class RedisConfigurationProvider : ConfigurationProvider, IDisposable
{
    private readonly RedisConfigurationSource _source;
    private readonly RedisConfigurationOptions _options;
    private RedisConnectionProvider? _connectionProvider;
    private IRedisConnection? _connection;
    private IConnectionMultiplexer? _multiplexer;
    private ISubscriber? _subscriber;
    private Timer? _pollingTimer;
    private string? _lastHash;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of <see cref="RedisConfigurationProvider"/>.
    /// </summary>
    /// <param name="source">The source settings.</param>
    public RedisConfigurationProvider(RedisConfigurationSource source)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _options = source.Options;
    }

    /// <summary>
    /// Loads configuration data from Redis.
    /// </summary>
    public override void Load()
    {
        try
        {
            InitializeConnection();
            LoadConfigurationData();
            SetupReloadMechanism();
        }
        catch when (_options.Optional)
        {
            Data = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
            // Log or handle the exception as appropriate for optional providers
        }
    }

    private void InitializeConnection()
    {
        _connectionProvider = new RedisConnectionProvider(_options.ConnectionString);
        _connection = _connectionProvider.Connection;

        // Get the underlying StackExchange.Redis connection for pub/sub support
        var connectionInfo = ConnectionMultiplexer.Connect(_options.ConnectionString);
        _multiplexer = connectionInfo;
    }

    private void LoadConfigurationData()
    {
        if (_multiplexer == null)
            return;

        var database = _multiplexer.GetDatabase();
        string? json;

        if (_options.StorageMode == StorageMode.RedisJson)
        {
            var jsonCommands = database.JSON();
            var result = jsonCommands.Get(_options.ConfigurationKey);
            json = result?.ToString();
        }
        else
        {
            var jsonValue = database.StringGet(_options.ConfigurationKey);
            json = jsonValue.IsNullOrEmpty ? null : jsonValue.ToString();
        }

        if (string.IsNullOrEmpty(json))
        {
            Data = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
            _lastHash = null;
            return;
        }

        var currentHash = ComputeHash(json);

        // Only update if the configuration has changed
        if (_lastHash == currentHash)
            return;

        _lastHash = currentHash;
        Data = ParseJson(json);
    }

    private static Dictionary<string, string?> ParseJson(string json)
    {
        var data = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        try
        {
            using var document = JsonDocument.Parse(json);
            ParseElement(document.RootElement, string.Empty, data);
        }
        catch (JsonException)
        {
            // If the JSON is invalid, return empty configuration
            return data;
        }

        return data;
    }

    private static void ParseElement(JsonElement element, string prefix, Dictionary<string, string?> data)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var property in element.EnumerateObject())
                {
                    var key = string.IsNullOrEmpty(prefix)
                        ? property.Name
                        : $"{prefix}{ConfigurationPath.KeyDelimiter}{property.Name}";
                    ParseElement(property.Value, key, data);
                }
                break;

            case JsonValueKind.Array:
                var index = 0;
                foreach (var item in element.EnumerateArray())
                {
                    var key = $"{prefix}{ConfigurationPath.KeyDelimiter}{index}";
                    ParseElement(item, key, data);
                    index++;
                }
                break;

            case JsonValueKind.String:
                data[prefix] = element.GetString();
                break;

            case JsonValueKind.Number:
                data[prefix] = element.GetRawText();
                break;

            case JsonValueKind.True:
            case JsonValueKind.False:
                data[prefix] = element.GetBoolean().ToString().ToLowerInvariant();
                break;

            case JsonValueKind.Null:
                data[prefix] = null;
                break;
        }
    }

    private void SetupReloadMechanism()
    {
        switch (_options.ReloadMode)
        {
            case ReloadMode.Polling:
                SetupPolling();
                break;

            case ReloadMode.PubSub:
                SetupPubSub();
                break;

            case ReloadMode.None:
            default:
                // No reload mechanism
                break;
        }
    }

    private void SetupPolling()
    {
        _pollingTimer = new Timer(
            _ => PollForChanges(),
            null,
            _options.PollingInterval,
            _options.PollingInterval);
    }

    private void PollForChanges()
    {
        try
        {
            var previousData = Data;
            LoadConfigurationData();

            if (!DataEquals(previousData, Data))
            {
                OnReload();
            }
        }
        catch
        {
            // Swallow exceptions during polling to prevent crashes
            // Consider adding logging here
        }
    }

    private static bool DataEquals(IDictionary<string, string?>? first, IDictionary<string, string?>? second)
    {
        if (first == null && second == null) return true;
        if (first == null || second == null) return false;
        if (first.Count != second.Count) return false;

        foreach (var kvp in first)
        {
            if (!second.TryGetValue(kvp.Key, out var value) || value != kvp.Value)
                return false;
        }

        return true;
    }

    private void SetupPubSub()
    {
        if (_multiplexer == null)
            return;

        _subscriber = _multiplexer.GetSubscriber();
        _subscriber.Subscribe(
            RedisChannel.Literal(_options.PubSubChannel),
            (_, _) => OnConfigurationChanged());
    }

    private void OnConfigurationChanged()
    {
        try
        {
            LoadConfigurationData();
            OnReload();
        }
        catch
        {
            // Swallow exceptions during reload to prevent crashes
            // Consider adding logging here
        }
    }

    private static string ComputeHash(string input)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        var hashBytes = System.Security.Cryptography.SHA256.HashData(bytes);
        return Convert.ToBase64String(hashBytes);
    }

    /// <summary>
    /// Releases the resources used by this provider.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by this provider and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _pollingTimer?.Dispose();
            _subscriber?.UnsubscribeAll();
            _multiplexer?.Dispose();
        }

        _disposed = true;
    }
}
