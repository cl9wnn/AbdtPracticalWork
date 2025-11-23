using PracticalWork.Library.Enums;

namespace PracticalWork.Library.Dtos;

/// <summary>
/// DTO для фильтрации списка книг
/// </summary>
public class BookFilterDto
{
    /// <summary>Категория книги</summary>
    public BookCategory Category { get; set; }
    
    /// <summary>Автор книги</summary>
    public string Author { get; set; }
    
    /// <summary>Статус книги</summary>
    public BookStatus? Status { get; set; }
    
    /// <summary>Доступность книги</summary>
    public bool? AvailableOnly { get; set; }
}