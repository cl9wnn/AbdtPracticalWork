using PracticalWork.Reports.Contracts.v1.Enums;

namespace PracticalWork.Reports.Contracts.v1.ActivityLogs.Get;

/// <summary>
/// Запрос на получение списка логов активности
/// </summary>
/// <param name="EventType">Тип активности</param>
/// <param name="EventDate">Дата активности</param>
/// <param name="Page">Страница</param>
/// <param name="PageSize">Размер страницы</param>
public sealed record GetActivityLogsRequest(
    ActivityEventType? EventType, 
    DateTime? EventDate, 
    int Page, 
    int PageSize);