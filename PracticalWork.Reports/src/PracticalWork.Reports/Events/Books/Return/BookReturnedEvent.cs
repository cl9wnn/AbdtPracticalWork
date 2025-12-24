using System.Text.Json;
using PracticalWork.Reports.Enums;
using PracticalWork.Reports.Events.Abstractions;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Events.Books.Return;

/// <summary>
/// Событие возврата книги в библиотеку
/// </summary>
/// <param name="BookId">Уникальный идентификатор книги</param>
/// <param name="ReaderId">Уникальный идентификатор читателя</param>
/// <param name="BookTitle">Название книги</param>
/// <param name="ReaderName">ФИО читателя</param>
/// <param name="ReturnDate">Дата возврата книги</param>
public sealed record BookReturnedEvent(
    Guid BookId,
    Guid ReaderId,
    string BookTitle,
    string ReaderName,
    DateOnly? ReturnDate
    ) : BaseLibraryEvent("book.returned"), IActivityLoggable
{
    public ActivityLog ToActivityLog()
    {
        var metadata = JsonDocument.Parse(JsonSerializer.Serialize(new
        {
            BookId,
            ReaderId,
            BookTitle,
            ReaderName,
            ReturnDate
        }));

        return ActivityLog.Create(ActivityEventType.BookReturned, metadata);
    }
}