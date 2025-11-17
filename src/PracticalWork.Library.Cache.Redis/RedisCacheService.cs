using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using PracticalWork.Library.Abstractions.Services.Infrastructure;
using StackExchange.Redis;

namespace PracticalWork.Library.Cache.Redis;

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

    public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var cachedData = await _cache.GetStringAsync(key, token: cancellationToken);

        if (string.IsNullOrEmpty(cachedData))
        {
            return default;
        }
        
        return JsonSerializer.Deserialize<T>(cachedData, _jsonSerializerOptions);
    }

    public async Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(key, cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        var value = await _cache.GetStringAsync(key, token: cancellationToken);
        return value != null;
    }

    public async Task<int> GetVersionAsync(string key, CancellationToken cancellationToken = default)
    {
        var db = _redis.GetDatabase();
        var redisKey = $"{_prefix}{key}:version";

        var version = await db.StringGetAsync(redisKey);

        return version.IsNullOrEmpty ? 1 : (int)version;
    }

    public async Task<int> IncrementVersionAsync(string key, CancellationToken cancellationToken = default)
    {
        var db = _redis.GetDatabase();
        var redisKey = $"{_prefix}{key}:version";

        return (int)await db.StringIncrementAsync(redisKey);
    }
}