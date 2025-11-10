namespace PracticalWork.Library.Abstractions.Services.Infrastructure;

/// <summary>
/// Определяет контракт для сервиса кэширования данных.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Сохраняет значение в кэше по заданному ключу.
    /// </summary>
    /// <typeparam name="T">Тип сохраняемого значения</typeparam>
    /// <param name="key">Ключ кэша</param>
    /// <param name="value">Сохраняемое значение</param>
    /// <param name="ttl">Время жизни (Time-To-Live). Если не указано — кэш бессрочный</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task SetAsync<T>(string key, T value, TimeSpan? ttl = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получает значение из кэша по ключу.
    /// </summary>
    /// <typeparam name="T">Тип извлекаемого значения</typeparam>
    /// <param name="key">Ключ кэша</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Значение из кэша</returns>
    Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаляет значение из кэша по ключу.
    /// </summary>
    /// <param name="key">Ключ удаляемого значения</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns><c>true</c>, если значение удалено; иначе — <c>false</c></returns>
    Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверяет, существует ли значение с заданным ключом в кэше.
    /// </summary>
    /// <param name="key">Ключ для проверки</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns><c>true</c>, если значение существует; иначе — <c>false</c></returns>
    Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);
}