using System.Text.Json;
using PracticalWork.Reports.Enums;
using PracticalWork.Reports.Events.Abstractions;
using PracticalWork.Reports.Models;

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
    ) : BaseLibraryEvent("reader.closed"), IActivityLoggable
{
    public ActivityLog ToActivityLog()
    {
        var metadata = JsonDocument.Parse(JsonSerializer.Serialize(new
        {
            ReaderId,
            FullName,
            ClosedAt
        }));

        return ActivityLog.Create(ActivityEventType.ReaderClosed, metadata);
    }
}