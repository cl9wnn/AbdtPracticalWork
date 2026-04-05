namespace PracticalWork.Library.Dtos;

/// <summary>
/// Отчет о плановой архивации книг
/// </summary>
public class ArchiveReportDto
{
    /// <summary>
    /// Общее количество обработанных книг, соответствующих критериям архивации
    /// </summary>
    public int TotalProcessed { get; set; }
    
    /// <summary>
    /// Количество успешно архивированных
    /// </summary>
    public int SuccessfullyArchived { get; set; }
    
    /// <summary>
    /// Количество пропущенных
    /// </summary>
    public int Skipped { get; set; }

    /// <summary>
    /// Общее время выполнения
    /// </summary>
    public long ExecutionTimeMs { get; set; }
    
    /// <summary>
    /// Детали отмены архивации книг
    /// </summary>
    public List<ArchiveSkipDetailDto> SkippedDetails { get; set; } = new();

}