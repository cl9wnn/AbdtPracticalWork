using System.Text.Json;
using PracticalWork.Reports.Enums;
using PracticalWork.Reports.Events.Abstractions;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Events.Books.Archive;

/// <summary>
/// Событие архивации книги в библиотеке
/// </summary>
/// <param name="BookId">Уникальный идентификатор книги</param>
/// <param name="Title">Название книги</param>
/// <param name="ArchivedAt">Дата и время архивации</param>
public sealed record BookArchivedEvent(
    Guid BookId,
    string Title,
    DateTime ArchivedAt
    ) : BaseLibraryEvent("book.archived"), IActivityLoggable
{
    public ActivityLog ToActivityLog()
    {
        var metadata = JsonDocument.Parse(JsonSerializer.Serialize(new
        {
            BookId,
            Title,
            ArchivedAt
        }));

        return ActivityLog.Create(ActivityEventType.BookArchived, metadata);
    }
}