namespace PracticalWork.Reports.Dtos;

/// <summary>
/// Еженедельный отчет
/// </summary>
public class WeeklyReportDto
{
    /// <summary>
    /// Название отчета
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Ссылка на скачивание отчета
    /// </summary>
    public string DownloadUrl { get; set; }
    
    /// <summary>
    /// Дата генерации
    /// </summary>
    public DateTime GeneratedAt { get; set; }
}