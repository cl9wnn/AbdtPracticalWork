using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PracticalWork.Library.SharedKernel.Helpers;

/// <summary>Предоставляет вспомогательные методы для генерации уникальных кеш-ключей</summary>
public static class CacheKeyHasher
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    /// <summary>
    /// Генерирует детерминированный ключ, основанный на заданном префиксе, версии кэша и хэше модели.
    /// </summary>
    /// <param name="prefix">Префикс ключа</param>
    /// <param name="cacheVersion">Версия кэша</param>
    /// <param name="model">Объект, по которому вычисляется хэш для формирования ключа</param>
    /// <typeparam name="T">Тип модели, используемой для вычисления хэша</typeparam>
    /// <returns>Строка кэш-ключа формата: <c>{prefix}:v{cacheVersion}:{hash}</c></returns>
    public static string GenerateCacheKey<T>(string prefix, int cacheVersion, T model)
    {
        var serializedModel = JsonSerializer.Serialize(model, SerializerOptions);
        var hash = ComputeHash(serializedModel);
        return $"{prefix}:v{cacheVersion}:{hash}";
    }
    
    /// <summary>
    /// Вычисляет хэш для заданной строки и возвращает его в виде hex-представления.
    /// </summary>
    /// <param name="input">Строка, для которой требуется вычислить хэш</param>
    /// <returns>Хэш в виде строки шестнадцатеричных символов</returns>
    private static string ComputeHash(string input)
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
}