using PracticalWork.Library.SharedKernel.Abstractions;

namespace PracticalWork.Library.SharedKernel.Entities;

/// <summary>
/// Базовый класс для всех сущностей
/// </summary>
public abstract class EntityBase : IEntity
{
    /// <summary> Идентификатор сущности </summary>
    public Guid Id { get; set; }

    /// <summary> Дата создания </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary> Дата обновления </summary>
    public DateTime? UpdatedAt { get; set; }

    protected EntityBase()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }
}
