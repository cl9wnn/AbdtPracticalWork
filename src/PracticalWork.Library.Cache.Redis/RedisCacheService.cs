using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using PracticalWork.Library.Abstractions.Services.Infrastructure;
using StackExchange.Redis;

namespace PracticalWork.Library.Cache.Redis;

/// <summary>
/// Сервис кэширования данных с помощью распределенного сервиса Redis
/// </summary>
public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly IConnectionMultiplexer _redis;
    private readonly string _prefix;
    
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        ReferenceHandler = ReferenceHandler.Preserve,
        WriteIndented = false
    };

    public RedisCacheService(IDistributedCache cache, IConnectionMultiplexer redisDb, IConfiguration configuration)
    {
        _cache = cache;
        _redis = redisDb;
        _prefix = configuration["App:Redis:RedisCachePrefix"] ?? "";
    }

    /// <inheritdoc cref="ICacheService.SetAsync{T}"/>
    public async Task SetAsync<T>(string key, T value, TimeSpan? ttl = null,
        CancellationToken cancellationToken = default)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var serializedData = JsonSerializer.Serialize(value, _jsonSerializerOptions);
        var options = new DistributedCacheEntryOptions();

        if (ttl.HasValue)
        {
            options.SetSlidingExpiration(ttl.Value);
        }

        await _cache.SetStringAsync(key, serializedData, options, token: cancellationToken);
    }

    /// <inheritdoc cref="ICacheService.GetAsync{T}"/>
    public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var cachedData = await _cache.GetStringAsync(key, token: cancellationToken);

        if (string.IsNullOrEmpty(cachedData))
        {
            return default;
        }
        
        return JsonSerializer.Deserialize<T>(cachedData, _jsonSerializerOptions);
    }

    /// <inheritdoc cref="ICacheService.RemoveAsync"/>
    public async Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(key, cancellationToken);
        return true;
    }

    /// <inheritdoc cref="ICacheService.ExistsAsync"/>
    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        var value = await _cache.GetStringAsync(key, token: cancellationToken);
        return value != null;
    }

    /// <inheritdoc cref="ICacheService.GetVersionAsync"/>
    public async Task<int> GetVersionAsync(string key, CancellationToken cancellationToken = default)
    {
        var db = _redis.GetDatabase();
        var redisKey = $"{_prefix}{key}";

        var version = await db.StringGetAsync(redisKey);

        return version.IsNullOrEmpty ? 1 : (int)version;
    }

    /// <inheritdoc cref="ICacheService.IncrementVersionAsync"/>
    public async Task<int> IncrementVersionAsync(string key, CancellationToken cancellationToken = default)
    {
        var db = _redis.GetDatabase();
        var redisKey = $"{_prefix}{key}";

        return (int)await db.StringIncrementAsync(redisKey);
    }
}