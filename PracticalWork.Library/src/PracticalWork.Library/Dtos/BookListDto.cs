namespace PracticalWork.Library.Dtos;

/// <summary>
/// DTO для представления книги из списка
/// </summary>
public class BookListDto
{
    /// <summary>Название книги</summary>
    public string Title { get; set; } 
    
    /// <summary>Список авторов</summary>
    public IReadOnlyList<string> Authors { get; set; }
    
    /// <summary>Описание книги</summary>
    public string Description { get; set; }
    
    /// <summary>Год издания книги</summary>
    public int Year { get; set; }
}