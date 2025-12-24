using System.Text.Json;
using PracticalWork.Reports.Enums;

namespace PracticalWork.Reports.Models;

/// <summary>
/// Лог активности
/// </summary>
public sealed class ActivityLog
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

    public static ActivityLog Create(ActivityEventType eventType ,JsonDocument metadata)
    {
        return new ActivityLog
        {
            EventType = eventType, 
            EventDate = DateTime.UtcNow, 
            Metadata = metadata
        };
    }
}