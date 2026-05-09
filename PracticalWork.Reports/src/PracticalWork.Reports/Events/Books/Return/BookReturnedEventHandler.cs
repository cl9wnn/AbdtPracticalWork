using System.Text.Json;
using PracticalWork.Reports.Abstractions.Storage;
using PracticalWork.Reports.Enums;
using PracticalWork.Reports.Models;
using PracticalWork.Shared.Abstractions.Interfaces;

namespace PracticalWork.Reports.Events.Books.Return;

/// <summary>
/// Обработчик события возврата книги в библиотеку
/// </summary>
public class BookReturnedEventHandler: IEventHandler<BookReturnedEvent>
{
    private readonly IActivityLogRepository _activityLogRepository;

    public BookReturnedEventHandler(IActivityLogRepository activityLogRepository)
    {
        _activityLogRepository = activityLogRepository;
    }

    /// <inheritdoc cref="IEventHandler{T}.HandleAsync"/>
    public async Task HandleAsync(BookReturnedEvent message, CancellationToken cancellationToken)
    {
        var metadata = JsonDocument.Parse(JsonSerializer.Serialize(message));
        var activityLog = ActivityLog.Create(ActivityEventType.BookReturned, metadata);
        
        await _activityLogRepository.Add(activityLog, cancellationToken,
            bookId: message.BookId, readerId: message.ReaderId);
    }
}