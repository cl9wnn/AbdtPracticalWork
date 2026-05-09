using System.Text.Json;
using PracticalWork.Reports.Abstractions.Storage;
using PracticalWork.Reports.Enums;
using PracticalWork.Reports.Models;
using PracticalWork.Shared.Abstractions.Interfaces;

namespace PracticalWork.Reports.Events.Books.Borrow;

/// <summary>
/// Обработчик события выдачи книги читателю
/// </summary>
public class BookBorrowedEventHandler: IEventHandler<BookBorrowedEvent>
{
    private readonly IActivityLogRepository _activityLogRepository;

    public BookBorrowedEventHandler(IActivityLogRepository activityLogRepository)
    {
        _activityLogRepository = activityLogRepository;
    }
    
    /// <inheritdoc cref="IEventHandler{T}.HandleAsync"/>
    public async Task HandleAsync(BookBorrowedEvent message, CancellationToken cancellationToken)
    {
        var metadata = JsonDocument.Parse(JsonSerializer.Serialize(message));
        var activityLog = ActivityLog.Create(ActivityEventType.BookBorrowed, metadata);
        
        await _activityLogRepository.Add(activityLog, cancellationToken,
            bookId: message.BookId, readerId: message.ReaderId);
    }
}