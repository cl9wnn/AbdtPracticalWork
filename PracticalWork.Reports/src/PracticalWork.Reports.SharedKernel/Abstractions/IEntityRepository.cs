namespace PracticalWork.Reports.SharedKernel.Abstractions;

/// <summary>
/// Универсальный интерфейс репозитория, предоставляющий базовые операции с сущностью в хранилище
/// </summary>
/// <typeparam name="TKey">Тип уникального идентификатора сущности (например, Guid)</typeparam>
/// <typeparam name="TDto">Тип объекта передачи данных (DTO), представляющего сущность</typeparam>
public interface IEntityRepository<in TKey, TDto>
{
    /// <summary>
    /// Возвращает сущность по ее идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>DTO найденной сущности</returns>
    Task<TDto> GetById(TKey id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Добавляет новую сущность в хранилище.
    /// </summary>
    /// <param name="dto">DTO добавляемой сущности</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Идентификатор созданной сущности</returns>
    Task<Guid> Add(TDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновляет сущность в хранилище.
    /// </summary>
    /// <param name="id">Идентификатор изменяемой сущности</param>
    /// <param name="dto">DTO обновляемой сущности</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Изменённое DTO</returns>
    Task<TDto> Update(TKey id, TDto dto, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Удаляет сущность из хранилища.
    /// </summary>
    /// <param name="id">Идентификатор удаляемой сущности</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task Delete(TKey id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Возвращает список сущностей из хранилища.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список DTO</returns>
    Task<ICollection<TDto>> GetAll(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Проверяет существование сущности по ID.
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task<bool> Exists(TKey id, CancellationToken cancellationToken = default);
}