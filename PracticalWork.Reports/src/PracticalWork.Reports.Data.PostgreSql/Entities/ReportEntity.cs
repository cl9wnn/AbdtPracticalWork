using PracticalWork.Reports.Enums;
using PracticalWork.Reports.SharedKernel.Entities;

namespace PracticalWork.Reports.Data.PostgreSql.Entities;

/// <summary>
/// Отчет
/// </summary>
public class ReportEntity: EntityBase
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