namespace PracticalWork.Library.Dtos;

/// <summary>
/// Информация о пропуске книги на плановой архивации
/// </summary>
public class ArchiveSkipDetailDto
{
    /// <summary>
    /// Идентификатор книги, пропустившей архивацию
    /// </summary>
    public Guid BookId { get; set; }
    
    /// <summary>
    /// Причина пропуска архивации книги
    /// </summary>
    public string Reason { get; set; }
}