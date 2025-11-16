namespace PracticalWork.Library.SharedKernel.Abstractions;

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
    /// <returns>DTO найденной сущности</returns>
    Task<TDto> GetById(TKey id);
    
    /// <summary>
    /// Добавляет новую сущность в хранилище.
    /// </summary>
    /// <param name="dto">DTO добавляемой сущности</param>
    /// <returns>Идентификатор созданной сущности</returns>
    Task<Guid> Add(TDto dto);

    /// <summary>
    /// Обновляет сущность в хранилище.
    /// </summary>
    /// <param name="id">Идентификатор изменяемой сущности</param>
    /// <param name="dto">DTO обновляемой сущности</param>
    /// <returns>Изменённое DTO</returns>
    Task<TDto> Update(TKey id, TDto dto);
    
    /// <summary>
    /// Удаляет сущность из хранилища.
    /// </summary>
    /// <param name="id">Идентификатор удаляемой сущности</param>
    Task Delete(TKey id);
    
    /// <summary>
    /// Возвращает список сущностей из хранилища.
    /// </summary>
    /// <returns>Список DTO</returns>
    Task<ICollection<TDto>> GetAll();
    
    /// <summary>
    /// Проверяет существование сущности по ID.
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    Task<bool> Exists(TKey id);
}