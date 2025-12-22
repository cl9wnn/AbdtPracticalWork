namespace PracticalWork.Reports.Contracts.v1.Enums;

/// <summary>
/// Статус отчета
/// </summary>
public enum ReportStatus
{
    /// <summary>
    /// В процессе
    /// </summary>
    InProgress = 0,
    
    /// <summary>
    /// Сгенерирован
    /// </summary>
    Generated = 10,
    
    /// <summary>
    /// Ошибка
    /// </summary>
    Error = 20
}