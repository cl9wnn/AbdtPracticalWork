using PracticalWork.Reports.Events.Books;
using PracticalWork.Reports.Events.Books.Archive;
using PracticalWork.Reports.Events.Books.Create;
using PracticalWork.Reports.Events.Books.Return;
using PracticalWork.Reports.Events.Readers.Close;
using PracticalWork.Reports.Events.Readers.Create;
using PracticalWork.Reports.SharedKernel.Events;

namespace PracticalWork.Reports.MessageBroker.Kafka;

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