namespace PracticalWork.Reports.Options;

/// <summary>
/// Настройки кэширования для функциональности отчетов книг
/// </summary>
public class BooksCacheOptions
{
    /// <summary>
    /// Общий префикс для инвалидации кэша
    /// </summary>
    public string ReportsCacheVersionPrefix { get; set; }  
    
    /// <summary>
    /// Настройки кэширования для списков отчетов книг
    /// </summary>
    public CacheEntryOptions ReportsListCache { get; set; } 
}