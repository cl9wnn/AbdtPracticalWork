namespace PracticalWork.Shared.Abstractions.Interfaces;

/// <summary>
/// Универсальный интерфейс сервиса для работы с доменными моделями
/// </summary>
/// <typeparam name="TDto">Тип объекта передачи данных (DTO), представляющего модель</typeparam>
public interface IEntityService<TDto>
{
    /// <summary>
    /// Возвращает объект по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Объект</returns>
    Task<TDto> GetById(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверяет существование объекта по ID.
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task Exists(Guid id, CancellationToken cancellationToken = default);
}