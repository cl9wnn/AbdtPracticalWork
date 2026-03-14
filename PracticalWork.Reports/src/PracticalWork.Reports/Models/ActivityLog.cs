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
    public DateOnly EventDate { get; set; }

    /// <summary>
    /// Дополнительная информация
    /// </summary>
    public JsonDocument Metadata { get; set; }

    /// <summary>
    /// Создание лога активности
    /// </summary>
    /// <param name="eventType">Тип активности лога</param>
    /// <param name="metadata">Дополнительная информация</param>
    /// <returns>Новый экземпляр лога активности</returns>
    public static ActivityLog Create(ActivityEventType eventType ,JsonDocument metadata)
    {
        return new ActivityLog
        {
            EventType = eventType, 
            EventDate = DateOnly.FromDateTime(DateTime.UtcNow), 
            Metadata = metadata
        };
    }
}