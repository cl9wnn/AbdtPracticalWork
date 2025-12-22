using PracticalWork.Reports.Enums;

namespace PracticalWork.Reports.Dtos;

/// <summary>
/// DTO для фильтрации списка книг
/// </summary>
public class ActivityLogFilterDto
{
    public ActivityEventType? EventType { get; set; }
    public DateTime? EventDate { get; set; }
}