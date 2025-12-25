namespace PracticalWork.Reports.Dtos;

/// <summary>
/// DTO строки таблицы с логом активности
/// </summary>
public sealed class ActivityLogReportRow
{
    /// <summary>
    /// Дата события
    /// </summary>
    public DateTime EventDate { get; init; }
    
    /// <summary>
    /// ТИп события
    /// </summary>
    public string EventType { get; init; }
    
    /// <summary>
    /// Дополнительные данные лога
    /// </summary>
    public string Metadata { get; init; }
}