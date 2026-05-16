namespace PracticalWork.Library.Dtos;

/// <summary>
/// DTO для статистики активности библиотеки
/// </summary>
public class LibraryStatisticsDto
{
    /// <summary>
    /// Количество новых книг
    /// </summary>
    public int NewBooksCount { get; set; }
    
    /// <summary>
    /// Количество новых читателей
    /// </summary>
    public int NewReadersCount { get; set; }
    
    /// <summary>
    /// Количество выданных книг
    /// </summary>
    public int BorrowedBooksCount { get; set; }
    
    /// <summary>
    /// Количество возвращенных книг
    /// </summary>
    public int ReturnedBooksCount { get; set; }
    
    /// <summary>
    /// Количество просроченных выдач
    /// </summary>
    public int OverdueBooksCount { get; set; }
}