namespace PracticalWork.Library.Options.Jobs;

/// <summary>
/// Настройки автоматической архивации старых книг в библиотеке
/// </summary>
public class ArchiveSettings
{
    /// <summary>
    /// Количество лет, в течение которых книга не выдавалась, чтобы считаться
    /// кандидатом на архивацию.
    /// </summary>
    public int YearsWithoutBorrow { get; set; }
    /// <summary>
    /// Максимальное количество книг для обработки за один запуск задачи архивации.
    /// </summary>
    public int MaxBooksPerRun { get; set; }
}