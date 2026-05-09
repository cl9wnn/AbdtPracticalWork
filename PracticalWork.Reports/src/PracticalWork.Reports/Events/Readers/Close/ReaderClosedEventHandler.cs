using System.Text.Json;
using PracticalWork.Reports.Abstractions.Storage;
using PracticalWork.Reports.Enums;
using PracticalWork.Reports.Models;
using PracticalWork.Shared.Abstractions.Interfaces;

namespace PracticalWork.Reports.Events.Readers.Close;

/// <summary>
/// Обработчик события закрытия карточки читателя
/// </summary>
public class ReaderClosedEventHandler: IEventHandler<ReaderClosedEvent>
{
    private readonly IActivityLogRepository _activityLogRepository;

    public ReaderClosedEventHandler(IActivityLogRepository activityLogRepository)
    {
        _activityLogRepository = activityLogRepository;
    }

    /// <inheritdoc cref="IEventHandler{T}.HandleAsync"/>
    public async Task HandleAsync(ReaderClosedEvent message, CancellationToken cancellationToken)
    {
        var metadata = JsonDocument.Parse(JsonSerializer.Serialize(message));
        var activityLog = ActivityLog.Create(ActivityEventType.ReaderClosed, metadata);
        
        await _activityLogRepository.Add(activityLog, cancellationToken, readerId: message.ReaderId);
    }
}