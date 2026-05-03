using PracticalWork.Reports.Events.Abstractions;

namespace PracticalWork.Reports.Events.Readers.Close;

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