namespace PracticalWork.Reports.SharedKernel.Abstractions;

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
    /// <returns>Объект</returns>
    Task<TDto> GetById(Guid id);
    
    /// <summary>
    /// Проверяет существование объекта по ID.
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    Task Exists(Guid id);
}