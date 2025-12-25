using System.Text.Json;
using PracticalWork.Reports.Contracts.v1.Enums;

namespace PracticalWork.Reports.Contracts.v1.ActivityLogs.Get;

/// <summary>
/// Ответ на запрос получения списка логов активности
/// </summary>
/// <param name="EventType">Тип события</param>
/// <param name="EventDate">Дата события</param>
/// <param name="Metadata">Дополнительная информация об активности</param>
public record GetActivityLogsResponse(ActivityEventType EventType, DateTime EventDate, JsonDocument Metadata);
