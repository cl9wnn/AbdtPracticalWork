using PracticalWork.Reports.Events.Abstractions;

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
    ) : BaseLibraryEvent("book.archived");