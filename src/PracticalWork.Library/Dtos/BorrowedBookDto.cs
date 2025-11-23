namespace PracticalWork.Library.Dtos;

/// <summary>
/// DTO для представления выданной читателю книги
/// </summary>
public class BorrowedBookDto
{
    /// <summary>Идентификатор книги</summary>
    public Guid BookId { get; set; }
    
    /// <summary>Дата выдачи книги</summary>
    public DateOnly BorrowDate { get; set; }
    
    /// <summary>Срок возврата книги</summary>
    public DateOnly DueDate { get; set; }
}