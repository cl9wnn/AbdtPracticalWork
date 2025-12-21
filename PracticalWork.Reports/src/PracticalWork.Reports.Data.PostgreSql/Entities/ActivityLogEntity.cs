using System.Text.Json;
using PracticalWork.Reports.Enums;
using PracticalWork.Reports.SharedKernel.Entities;

namespace PracticalWork.Reports.Data.PostgreSql.Entities;

/// <summary>
/// Лог активности
/// </summary>
public class ActivityLogEntity: EntityBase
{
    /// <summary>
    /// Тип события
    /// </summary>
    public ActivityEventType EventType { get; set; }

    /// <summary>
    /// Дата события 
    /// </summary>
    public DateTime EventDate { get; set; }

    /// <summary>
    /// Дополнительная информация
    /// </summary>
    public JsonDocument Metadata { get; set; }
    
    /// <summary>
    /// Внешний ключ на сущность книги
    /// </summary>
    public Guid? BookId { get; set; }
    
    /// <summary>
    /// Внешний ключ на сущность карточки читателя
    /// </summary>
    public Guid? ReaderId { get; set; }
}