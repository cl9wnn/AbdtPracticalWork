namespace PracticalWork.Reports.Abstractions.Services.Infrastructure;

/// <summary>
/// Контракт сервиса кэширования данных
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Сохраняет значение в кэше по заданному ключу.
    /// </summary>
    /// <param name="keyPrefix">Префикс ключа для кэша</param>
    /// <param name="cacheVersionPrefix">Префикс версии кэша</param>
    /// <param name="id">Идентификатор сохраняемого значения в кэше</param>
    /// <param name="value">Значение, сохраняющееся в кэш</param>
    /// <param name="ttl">Время жизни (Time-To-Live). Если не указано — кэш бессрочный</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <typeparam name="TValue">Тип сохраняемого объекта из кэша</typeparam>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    Task SetAsync<TKey, TValue>(string keyPrefix, string cacheVersionPrefix, TKey id, TValue value,
        TimeSpan? ttl = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Сохраняет значение в кэше по заданному ключу и объекту.
    /// </summary>
    /// <param name="keyPrefix">Префикс ключа для кэша</param>
    /// <param name="cacheVersionPrefix">Префикс версии кэша</param>
    /// <param name="model">Модель, по которой вычисляется хэш для ключа</param>
    /// <param name="value">Значение, сохраняющееся в кэш</param>
    /// <param name="ttl">Время жизни (Time-To-Live). Если не указано — кэш бессрочный</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <typeparam name="TValue">Тип сохраняемого объекта из кэша</typeparam>
    /// <typeparam name="TModel">Тип модели</typeparam>
    Task SetByModelAsync<TModel, TValue>(string keyPrefix, string cacheVersionPrefix, TModel model, TValue value,
        TimeSpan? ttl = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получает значение из кэша по ключу.
    /// </summary>
    /// <param name="keyPrefix">Префикс ключа для кэша</param>
    /// <param name="cacheVersionPrefix">Префикс версии кэша</param>
    /// <param name="id">Идентификатор значения из кэша</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <typeparam name="TValue">Тип возвращаемого объекта из кэша</typeparam>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    /// <returns>Значение из кэша</returns>
    Task<TValue> GetAsync<TKey, TValue>(string keyPrefix, string cacheVersionPrefix, TKey id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получает значение из кэша по ключу и объекту.
    /// </summary>
    /// <param name="keyPrefix">Префикс ключа для кэша</param>
    /// <param name="cacheVersionPrefix">Префикс версии кэша</param>
    /// <param name="model">Модель, по которой вычисляется хэш для ключа</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <typeparam name="TValue">Тип возвращаемого объекта из кэша</typeparam>
    /// <typeparam name="TModel">Тип модели</typeparam>
    /// <returns>Значение из кэша</returns>
    Task<TValue> GetByModelAsync<TModel, TValue>(string keyPrefix, string cacheVersionPrefix, TModel model,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Возвращает текущую версию кэша по заданному ключу
    /// </summary>
    /// <param name="keyPrefix">Префикс ключа для кэша</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Текущая версия кэша по заданному ключу</returns>
    Task<int> GetVersionAsync(string keyPrefix, CancellationToken cancellationToken = default);

    /// <summary>
    /// Атомарно увеличивает по заданному ключу на единицу
    /// </summary>
    /// <param name="keyPrefix">Префикс ключа для кэша</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Обновленная версия кэша по заданному ключу</returns>
    Task<int> IncrementVersionAsync(string keyPrefix, CancellationToken cancellationToken = default);
}