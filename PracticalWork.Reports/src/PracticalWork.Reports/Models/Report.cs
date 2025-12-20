using PracticalWork.Reports.Enums;

namespace PracticalWork.Reports.Models;

/// <summary>
/// Отчет
/// </summary>
public sealed class Report
{
    /// <summary>
    /// Название отчета
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Путь к файлу в MinIO
    /// </summary>
    public string FilePath { get; set; }
    
    /// <summary>
    /// Статус отчета
    /// </summary>
    public ReportStatus Status { get; set; }
    
    /// <summary>
    /// Дата генерации
    /// </summary>
    public DateTime GeneratedAt { get; set; }
    
    /// <summary>
    /// Начало периода отчета
    /// </summary>
    public DateOnly PeriodFrom { get; set; }
    
    /// <summary>
    ///  Конец периода отчета
    /// </summary>
    public DateOnly PeriodTo { get; set; }
}