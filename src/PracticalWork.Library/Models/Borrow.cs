using PracticalWork.Library.Enums;

namespace PracticalWork.Library.Models;

/// <summary>
/// Выдачи книги на руки читателю
/// </summary>
public sealed class Borrow
{
    /// <summary>Дата выдачи книги</summary>
    public DateOnly BorrowDate { get; set; }
    
    /// <summary>Срок возврата книги</summary>
    public DateOnly DueDate { get; set; }
    
    /// <summary>Фактическая дата возврата книги</summary>
    public DateOnly ReturnDate { get; set; }
    
    /// <summary>Статус книги в библиотеке</summary>
    public BookIssueStatus Status { get; set; }
}