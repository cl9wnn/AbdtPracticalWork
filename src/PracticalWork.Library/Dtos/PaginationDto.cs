namespace PracticalWork.Library.Dtos;

/// <summary>
/// DTO для пагинации данных
/// </summary>
public class PaginationDto
{
    /// <summary>Номер страницы</summary>
    public int Page { get; set; }
    
    /// <summary>Размер страницы</summary>
    public int PageSize { get; set; }
}