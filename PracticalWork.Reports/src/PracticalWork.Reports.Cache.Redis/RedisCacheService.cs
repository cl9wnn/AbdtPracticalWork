using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using PracticalWork.Reports.Abstractions.Services.Infrastructure;
using StackExchange.Redis;

namespace PracticalWork.Reports.Cache.Redis;

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

    /// <inheritdoc cref="ICacheService.SetAsync{TKey, TValue}"/>
    public async Task SetAsync<TKey, TValue>(string keyPrefix, string cacheVersionPrefix, TKey id, TValue value,
        TimeSpan? ttl = null, CancellationToken cancellationToken = default)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var cacheVersion = await GetVersionAsync(cacheVersionPrefix, cancellationToken);
        var key = BuildKey(keyPrefix, cacheVersion, id);
        
        var serializedData = JsonSerializer.Serialize(value, _jsonSerializerOptions);
        var options = new DistributedCacheEntryOptions();

        if (ttl.HasValue)
        {
            options.SetSlidingExpiration(ttl.Value);
        }

        await _cache.SetStringAsync(key, serializedData, options, token: cancellationToken);
    }

    /// <inheritdoc cref="ICacheService.SetByModelAsync{TModel, TValue}"/>
    public async Task SetByModelAsync<TModel, TValue>(string keyPrefix, string cacheVersionPrefix, TModel model, TValue value,
        TimeSpan? ttl = null, CancellationToken cancellationToken = default)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        
        var cacheVersion = await GetVersionAsync(cacheVersionPrefix, cancellationToken);
        var hashedKey = BuildKeyFromModel(keyPrefix, cacheVersion, model);
        
        var serializedData = JsonSerializer.Serialize(value, _jsonSerializerOptions);
        var options = new DistributedCacheEntryOptions();

        if (ttl.HasValue)
        {
            options.SetSlidingExpiration(ttl.Value);
        }

        await _cache.SetStringAsync(hashedKey, serializedData, options, token: cancellationToken);
    }
    
    /// <inheritdoc cref="ICacheService.GetAsync{TKey, TValue}"/>
    public async Task<TValue> GetAsync<TKey, TValue>(
        string keyPrefix,
        string cacheVersionPrefix,
        TKey id,
        CancellationToken cancellationToken = default)
    {
        var cacheVersion = await GetVersionAsync(cacheVersionPrefix, cancellationToken);
        var key = BuildKey(keyPrefix, cacheVersion, id);
        
        var cachedData = await _cache.GetStringAsync(key, token: cancellationToken);

        if (string.IsNullOrEmpty(cachedData))
        {
            return default;
        }

        return JsonSerializer.Deserialize<TValue>(cachedData, _jsonSerializerOptions);
    }

    /// <inheritdoc cref="ICacheService.GetByModelAsync{TModel, TValue}"/>
    public async Task<TValue> GetByModelAsync<TModel, TValue>(
        string keyPrefix,
        string cacheVersionPrefix, 
        TModel model,
        CancellationToken cancellationToken = default)
    {
        var cacheVersion = await GetVersionAsync(cacheVersionPrefix, cancellationToken);
        var hashedKey = BuildKeyFromModel(keyPrefix, cacheVersion, model);

        
        var cachedData = await _cache.GetStringAsync(hashedKey, token: cancellationToken);

        if (string.IsNullOrEmpty(cachedData))
        {
            return default;
        }

        return JsonSerializer.Deserialize<TValue>(cachedData, _jsonSerializerOptions);
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
    
    /// <summary>
    /// Вычисляет хэш для заданной строки и возвращает его в виде hex-представления.
    /// </summary>
    /// <param name="input">Строка, для которой требуется вычислить хэш</param>
    /// <returns>Хэш в виде строки шестнадцатеричных символов</returns>
    private string ComputeHash(string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = MD5.HashData(inputBytes);
        
        var sb = new StringBuilder();
        foreach (var bytes in hashBytes)
        {
            sb.Append(bytes.ToString("x2"));
        }
        
        return sb.ToString();
    }

    /// <summary>
    /// Строит ключ кэша по префиксу, версии и идентификатору 
    /// </summary>
    /// <param name="prefix">Префикс ключа для кэша</param>
    /// <param name="version">Версия кэша</param>
    /// <param name="id">Идентификатор значения в кэше</param>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    /// <returns>Ключ в виде строки</returns>
    private string BuildKey<TKey>(string prefix, long version, TKey id)
        => $"{prefix}:v{version}:{id}";

    /// <summary>
    /// Строит ключ кэша по префиксу, версии и модели
    /// </summary>
    /// <param name="prefix">Префикс ключа для кэша</param>
    /// <param name="version">Версия кэша</param>
    /// <param name="model">Модель, по которой вычисляется хэш</param>
    /// <typeparam name="TModel">Тип хэшируемой модели</typeparam>
    /// <returns>Ключ в виде строки</returns>
    private string BuildKeyFromModel<TModel>(string prefix, long version, TModel model)
    {
        var serialized = JsonSerializer.Serialize(model, _jsonSerializerOptions);
        var hash = ComputeHash(serialized);
        return $"{prefix}:v{version}:{hash}";
    }
}