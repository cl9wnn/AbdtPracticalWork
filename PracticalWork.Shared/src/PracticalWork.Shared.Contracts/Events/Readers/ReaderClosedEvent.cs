using PracticalWork.Shared.Contracts.Events.Abstractions;

namespace PracticalWork.Shared.Contracts.Events.Readers;

/// <summary>
/// Событие закрытия карточки читателя
/// </summary>
/// <param name="ReaderId">Уникальный идентификатор читателя</param>
/// <param name="FullName">Полное имя читателя</param>
/// <param name="ClosedAt">Дата и время закрытия карточки</param>
public sealed record ReaderClosedEvent(
    Guid ReaderId,
    string FullName,
    DateTime ClosedAt
    ) : BaseLibraryEvent("reader.closed");