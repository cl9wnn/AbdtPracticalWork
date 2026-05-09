using System.Text.Json;
using PracticalWork.Reports.Abstractions.Storage;
using PracticalWork.Reports.Enums;
using PracticalWork.Reports.Models;
using PracticalWork.Shared.Abstractions.Interfaces;

namespace PracticalWork.Reports.Events.Books.Archive;

/// <summary>
/// Обработчик события архивации книги в библиотеке
/// </summary>
public class BookArchivedEventHandler : IEventHandler<BookArchivedEvent>
{
    private readonly IActivityLogRepository _activityLogRepository;

    public BookArchivedEventHandler(IActivityLogRepository activityLogRepository)
    {
        _activityLogRepository = activityLogRepository;
    }

    /// <inheritdoc cref="IEventHandler{T}.HandleAsync"/>
    public async Task HandleAsync(BookArchivedEvent message, CancellationToken cancellationToken)
    {
        var metadata = JsonDocument.Parse(JsonSerializer.Serialize(message));
        var activityLog = ActivityLog.Create(ActivityEventType.BookArchived, metadata);
        
        await _activityLogRepository.Add(activityLog, cancellationToken, bookId: message.BookId);
    }
}