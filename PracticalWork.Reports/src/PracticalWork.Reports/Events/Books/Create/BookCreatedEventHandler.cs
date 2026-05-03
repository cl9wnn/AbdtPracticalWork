using System.Text.Json;
using PracticalWork.Reports.Abstractions.Storage;
using PracticalWork.Reports.Enums;
using PracticalWork.Reports.Models;
using PracticalWork.Reports.SharedKernel.Abstractions;

namespace PracticalWork.Reports.Events.Books.Create;

/// <summary>
/// Обработчик события создания новой книги в библиотеке
/// </summary>
public class BookCreatedEventHandler : IEventHandler<BookCreatedEvent>
{
    private readonly IActivityLogRepository _activityLogRepository;

    public BookCreatedEventHandler(IActivityLogRepository activityLogRepository)
    {
        _activityLogRepository = activityLogRepository;
    }

    /// <inheritdoc cref="IEventHandler{T}.HandleAsync"/>
    public async Task HandleAsync(BookCreatedEvent message, CancellationToken cancellationToken)
    {
        var metadata = JsonDocument.Parse(JsonSerializer.Serialize(message));
        var activityLog = ActivityLog.Create(ActivityEventType.BookCreated, metadata);
        
        await _activityLogRepository.Add(activityLog, cancellationToken, bookId: message.BookId);
    }
}