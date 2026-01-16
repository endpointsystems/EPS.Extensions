using System;
using EPS.Extensions.RedisConfig;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace EPS.Extensions.Test.RedisConfig;

public class RedisConfigurationOptionsTests
{
    [Fact]
    public void DefaultStorageMode_ShouldBeString()
    {
        var options = new RedisConfigurationOptions();

        Assert.Equal(StorageMode.String, options.StorageMode);
    }

    [Fact]
    public void DefaultConfigurationKey_ShouldBeAppConfiguration()
    {
        var options = new RedisConfigurationOptions();

        Assert.Equal("app:configuration", options.ConfigurationKey);
    }

    [Fact]
    public void DefaultReloadMode_ShouldBeNone()
    {
        var options = new RedisConfigurationOptions();

        Assert.Equal(ReloadMode.None, options.ReloadMode);
    }

    [Fact]
    public void DefaultPollingInterval_ShouldBe30Seconds()
    {
        var options = new RedisConfigurationOptions();

        Assert.Equal(TimeSpan.FromSeconds(30), options.PollingInterval);
    }

    [Fact]
    public void DefaultOptional_ShouldBeFalse()
    {
        var options = new RedisConfigurationOptions();

        Assert.False(options.Optional);
    }

    [Fact]
    public void StorageMode_CanBeSetToRedisJson()
    {
        var options = new RedisConfigurationOptions
        {
            StorageMode = StorageMode.RedisJson
        };

        Assert.Equal(StorageMode.RedisJson, options.StorageMode);
    }
}

public class RedisConfigurationSourceTests
{
    [Fact]
    public void Build_ReturnsRedisConfigurationProvider()
    {
        var source = new RedisConfigurationSource
        {
            Options = new RedisConfigurationOptions
            {
                ConnectionString = "localhost:6379",
                Optional = true // Set optional to avoid connection errors in tests
            }
        };

        var builder = new ConfigurationBuilder();
        var provider = source.Build(builder);

        Assert.IsType<RedisConfigurationProvider>(provider);
    }

    [Fact]
    public void Options_ArePreserved()
    {
        var options = new RedisConfigurationOptions
        {
            ConnectionString = "redis://myserver:6380",
            ConfigurationKey = "myapp:settings",
            StorageMode = StorageMode.RedisJson,
            ReloadMode = ReloadMode.Polling,
            PollingInterval = TimeSpan.FromMinutes(1),
            Optional = true
        };

        var source = new RedisConfigurationSource { Options = options };

        Assert.Equal("redis://myserver:6380", source.Options.ConnectionString);
        Assert.Equal("myapp:settings", source.Options.ConfigurationKey);
        Assert.Equal(StorageMode.RedisJson, source.Options.StorageMode);
        Assert.Equal(ReloadMode.Polling, source.Options.ReloadMode);
        Assert.Equal(TimeSpan.FromMinutes(1), source.Options.PollingInterval);
        Assert.True(source.Options.Optional);
    }
}

public class RedisConfigurationBuilderExtensionsTests
{
    [Fact]
    public void AddRedisConfiguration_WithConnectionString_AddsSource()
    {
        var builder = new ConfigurationBuilder();

        builder.AddRedisConfiguration("localhost:6379");

        Assert.Single(builder.Sources);
        Assert.IsType<RedisConfigurationSource>(builder.Sources[0]);
    }

    [Fact]
    public void AddRedisConfiguration_WithConnectionStringAndKey_SetsOptions()
    {
        var builder = new ConfigurationBuilder();

        builder.AddRedisConfiguration("localhost:6379", "custom:key");

        var source = Assert.IsType<RedisConfigurationSource>(builder.Sources[0]);
        Assert.Equal("localhost:6379", source.Options.ConnectionString);
        Assert.Equal("custom:key", source.Options.ConfigurationKey);
        Assert.Equal(StorageMode.String, source.Options.StorageMode);
    }

    [Fact]
    public void AddRedisConfiguration_WithOptionalFlag_SetsOptional()
    {
        var builder = new ConfigurationBuilder();

        builder.AddRedisConfiguration("localhost:6379", "app:config", optional: true);

        var source = Assert.IsType<RedisConfigurationSource>(builder.Sources[0]);
        Assert.True(source.Options.Optional);
    }

    [Fact]
    public void AddRedisConfigurationWithPolling_SetsPollingMode()
    {
        var builder = new ConfigurationBuilder();

        builder.AddRedisConfigurationWithPolling("localhost:6379", "app:config", TimeSpan.FromSeconds(15));

        var source = Assert.IsType<RedisConfigurationSource>(builder.Sources[0]);
        Assert.Equal(ReloadMode.Polling, source.Options.ReloadMode);
        Assert.Equal(TimeSpan.FromSeconds(15), source.Options.PollingInterval);
    }

    [Fact]
    public void AddRedisConfigurationWithPubSub_SetsPubSubMode()
    {
        var builder = new ConfigurationBuilder();

        builder.AddRedisConfigurationWithPubSub("localhost:6379", "app:config", "config:changed");

        var source = Assert.IsType<RedisConfigurationSource>(builder.Sources[0]);
        Assert.Equal(ReloadMode.PubSub, source.Options.ReloadMode);
        Assert.Equal("config:changed", source.Options.PubSubChannel);
    }

    [Fact]
    public void AddRedisJsonConfiguration_SetsRedisJsonStorageMode()
    {
        var builder = new ConfigurationBuilder();

        builder.AddRedisJsonConfiguration("localhost:6379");

        var source = Assert.IsType<RedisConfigurationSource>(builder.Sources[0]);
        Assert.Equal(StorageMode.RedisJson, source.Options.StorageMode);
    }

    [Fact]
    public void AddRedisJsonConfiguration_WithCustomKey_SetsOptions()
    {
        var builder = new ConfigurationBuilder();

        builder.AddRedisJsonConfiguration("localhost:6379", "appSettings");

        var source = Assert.IsType<RedisConfigurationSource>(builder.Sources[0]);
        Assert.Equal("localhost:6379", source.Options.ConnectionString);
        Assert.Equal("appSettings", source.Options.ConfigurationKey);
        Assert.Equal(StorageMode.RedisJson, source.Options.StorageMode);
    }

    [Fact]
    public void AddRedisJsonConfigurationWithPolling_SetsRedisJsonAndPolling()
    {
        var builder = new ConfigurationBuilder();

        builder.AddRedisJsonConfigurationWithPolling("localhost:6379", "app:config", TimeSpan.FromSeconds(20));

        var source = Assert.IsType<RedisConfigurationSource>(builder.Sources[0]);
        Assert.Equal(StorageMode.RedisJson, source.Options.StorageMode);
        Assert.Equal(ReloadMode.Polling, source.Options.ReloadMode);
        Assert.Equal(TimeSpan.FromSeconds(20), source.Options.PollingInterval);
    }

    [Fact]
    public void AddRedisJsonConfigurationWithPubSub_SetsRedisJsonAndPubSub()
    {
        var builder = new ConfigurationBuilder();

        builder.AddRedisJsonConfigurationWithPubSub("localhost:6379", "app:config", "json:changed");

        var source = Assert.IsType<RedisConfigurationSource>(builder.Sources[0]);
        Assert.Equal(StorageMode.RedisJson, source.Options.StorageMode);
        Assert.Equal(ReloadMode.PubSub, source.Options.ReloadMode);
        Assert.Equal("json:changed", source.Options.PubSubChannel);
    }

    [Fact]
    public void AddRedisConfiguration_WithDelegate_AllowsFullConfiguration()
    {
        var builder = new ConfigurationBuilder();

        builder.AddRedisConfiguration(options =>
        {
            options.ConnectionString = "redis://custom:6380";
            options.ConfigurationKey = "my:settings";
            options.StorageMode = StorageMode.RedisJson;
            options.ReloadMode = ReloadMode.Polling;
            options.PollingInterval = TimeSpan.FromMinutes(2);
            options.PubSubChannel = "my:channel";
            options.Optional = true;
            options.CommandTimeout = TimeSpan.FromSeconds(10);
        });

        var source = Assert.IsType<RedisConfigurationSource>(builder.Sources[0]);
        Assert.Equal("redis://custom:6380", source.Options.ConnectionString);
        Assert.Equal("my:settings", source.Options.ConfigurationKey);
        Assert.Equal(StorageMode.RedisJson, source.Options.StorageMode);
        Assert.Equal(ReloadMode.Polling, source.Options.ReloadMode);
        Assert.Equal(TimeSpan.FromMinutes(2), source.Options.PollingInterval);
        Assert.Equal("my:channel", source.Options.PubSubChannel);
        Assert.True(source.Options.Optional);
        Assert.Equal(TimeSpan.FromSeconds(10), source.Options.CommandTimeout);
    }
}

public class StorageModeEnumTests
{
    [Fact]
    public void StorageMode_HasStringValue()
    {
        Assert.Equal(0, (int)StorageMode.String);
    }

    [Fact]
    public void StorageMode_HasRedisJsonValue()
    {
        Assert.Equal(1, (int)StorageMode.RedisJson);
    }
}
