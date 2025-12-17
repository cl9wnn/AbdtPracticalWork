namespace PracticalWork.Library.Dtos;

/// <summary>
/// DTO для представления выданной читателю книги
/// </summary>
public class BorrowedBookDto
{
    /// <summary>Идентификатор книги</summary>
    public Guid BookId { get; set; }
    
    /// <summary>Название книги</summary>
    public string Title { get; set; }
    
    /// <summary>Авторы книги</summary>
    public IReadOnlyList<string> Authors { get; set; }
    
    /// <summary>Описание книги</summary>
    public string Description { get; set; }
    
    /// <summary>Год издания книги</summary>
    public int Year { get; set; }
    
    /// <summary>Дата выдачи книги</summary>
    public DateOnly BorrowDate { get; set; }
    
    /// <summary>Срок возврата книги</summary>
    public DateOnly DueDate { get; set; }
}