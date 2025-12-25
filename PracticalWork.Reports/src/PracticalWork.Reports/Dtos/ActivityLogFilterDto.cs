using PracticalWork.Reports.Enums;

namespace PracticalWork.Reports.Dtos;

/// <summary>
/// DTO для фильтрации списка книг
/// </summary>
public class ActivityLogFilterDto
{
    /// <summary>
    /// Тип события 
    /// </summary>
    public ActivityEventType? EventType { get; set; }
    
    /// <summary>
    /// Дата события
    /// </summary>
    public DateTime? EventDate { get; set; }
}