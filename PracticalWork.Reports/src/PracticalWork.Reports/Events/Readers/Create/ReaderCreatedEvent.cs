using System.Text.Json;
using PracticalWork.Reports.Enums;
using PracticalWork.Reports.Events.Abstractions;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Events.Readers.Create;

/// <summary>
/// Событие создания новой карточки читателя
/// </summary>
/// <param name="ReaderId">Уникальный идентификатор читателя</param>
/// <param name="FullName">Полное имя читателя</param>
/// <param name="PhoneNumber">Номер телефона читателя</param>
/// <param name="ExpiryDate">Дата окончания действия карточки</param>
/// <param name="CreatedAt">Дата и время создания карточки</param>
public sealed record ReaderCreatedEvent(
    Guid ReaderId,
    string FullName,
    string PhoneNumber,
    DateOnly ExpiryDate,
    DateTime CreatedAt
    ) : BaseLibraryEvent("reader.created"), IActivityLoggable
{
    public ActivityLog ToActivityLog()
    {
        var metadata = JsonDocument.Parse(JsonSerializer.Serialize(new
        {
            ReaderId,
            FullName,
            PhoneNumber,
            ExpiryDate,
            CreatedAt
        }));

        return ActivityLog.Create(ActivityEventType.BookCreated, metadata);
    }
}