using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PracticalWork.Reports.SharedKernel.Abstractions;

/// <summary>
/// Базовый интерфейс сущности
/// </summary>
public interface IEntity
{
    /// <summary> Идентификатор сущности </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    Guid Id { get; set; }

    /// <summary> Дата создания </summary>
    DateTime CreatedAt { get; set; }

    /// <summary> Дата обновления </summary>
    DateTime? UpdatedAt { get; set; }
}