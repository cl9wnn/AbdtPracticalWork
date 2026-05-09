using PracticalWork.Shared.Contracts.Events.Abstractions;

namespace PracticalWork.Shared.Contracts.Events.Books;

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