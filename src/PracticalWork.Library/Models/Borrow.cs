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
    public DateOnly? ReturnDate { get; set; }
    
    /// <summary>Статус книги в библиотеке</summary>
    public BookIssueStatus Status { get; set; }
    
    /// <summary>Проверка просроченности выдачи книги</summary>
    public bool IsOverdue() => ReturnDate > DueDate;

    /// <summary>
    /// Создание записи о выдаче книги на руки читателю
    /// </summary>
    /// <returns>Новая запись о выдаче</returns>
    public static Borrow Create()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        return new Borrow
        {
            BorrowDate = today,
            DueDate = today.AddDays(30),
            Status = BookIssueStatus.Issued,
        };
    }

    /// <summary>Возврат книги в библиотеку</summary>
    public void ReturnBook()
    {
        if (Status != BookIssueStatus.Issued)
        {
            throw new InvalidOperationException("Книга не выдана на руки читателю!");
        }
        
        ReturnDate = DateOnly.FromDateTime(DateTime.UtcNow);
        Status = IsOverdue() ? BookIssueStatus.Overdue : BookIssueStatus.Returned;
    }
}