using PracticalWork.Library.Enums;

namespace PracticalWork.Library.Dtos;

/// <summary>
/// DTO для представления деталей книги
/// </summary>
public class BookDetailsDto
{
    /// <summary>Идентификатор книги</summary>
    public Guid Id { get; set; }
    
    /// <summary>Название книги</summary>
    public string Title { get; set; }
    
    /// <summary>Категория книги</summary>
    public BookCategory Category { get; set; }
    
    /// <summary>Авторы книги</summary>
    public IReadOnlyList<string> Authors { get; set; }
    
    /// <summary>Описание книги</summary>
    public string Description { get; set; }
    
    /// <summary>Год издания книги</summary>
    public int Year { get; set; }
    
    /// <summary>Путь к изображению обложки</summary>
    public string CoverImagePath { get; set; }
    
    /// <summary>Статус книги</summary>
    public BookStatus Status { get; set; }
    
    /// <summary>В архиве</summary>
    public bool IsArchived {get; set;}
}