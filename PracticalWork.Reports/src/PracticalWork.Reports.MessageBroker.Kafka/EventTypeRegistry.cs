using PracticalWork.Shared.Abstractions.Events;
using PracticalWork.Shared.Contracts.Events.Books;
using PracticalWork.Shared.Contracts.Events.Readers;

namespace PracticalWork.Reports.MessageBroker.Kafka;

/// <summary>
/// Реестр для сопоставления строковых типов событий с типами .NET
/// </summary>
public sealed class EventTypeRegistry : IEventTypeRegistry
{
    private readonly Dictionary<string, Type> _types = new()
    {
        ["book.created"]  = typeof(BookCreatedEvent),
        ["book.archived"] = typeof(BookArchivedEvent),
        ["book.borrowed"] = typeof(BookBorrowedEvent),
        ["book.returned"] = typeof(BookReturnedEvent),
        ["reader.created"] = typeof(ReaderCreatedEvent),
        ["reader.closed"] = typeof(ReaderClosedEvent)
    };

    /// <inheritdoc cref="IEventTypeRegistry.GetEventType"/>
    public Type GetEventType(string eventType)
    {
        if (!_types.TryGetValue(eventType, out var type))
            throw new InvalidOperationException($"Unknown event type: {eventType}");

        return type;
    }
}