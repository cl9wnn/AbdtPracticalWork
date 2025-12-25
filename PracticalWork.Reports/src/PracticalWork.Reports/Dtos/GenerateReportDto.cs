using PracticalWork.Reports.Enums;

namespace PracticalWork.Reports.Dtos;

/// <summary>
/// DTO для генерации отчета
/// </summary>
public class GenerateReportDto
{
    /// <summary>
    /// Начало периода отчетности
    /// </summary>
    public DateOnly PeriodFrom { get; set; }
    
    /// <summary>
    /// Конец периода отчетности
    /// </summary>
    public DateOnly PeriodTo { get; set; }
    
    /// <summary>
    /// Тип события
    /// </summary>
    public ActivityEventType EventType { get; set; }
}