namespace PracticalWork.Library.Dtos;

/// <summary>
/// Фильтр поиска книг
/// </summary>
public class SearchBooksDto
{
    /// <summary>
    /// DTO для пагинации данных
    /// </summary>
    public PaginationDto Pagination { get; set; }
    
    /// <summary>
    /// DTO для фильтрации списка книг
    /// </summary>
    public BookFilterDto BookFilter { get; set; }
}