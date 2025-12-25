using PracticalWork.Reports.Abstractions.Storage;
using PracticalWork.Reports.SharedKernel.Abstractions;

namespace PracticalWork.Reports.Events.Readers.Create;

public class ReaderCreatedEventHandler: IEventHandler<ReaderCreatedEvent>
{
    private readonly IActivityLogRepository _activityLogRepository;

    public ReaderCreatedEventHandler(IActivityLogRepository activityLogRepository)
    {
        _activityLogRepository = activityLogRepository;
    }

    /// <inheritdoc cref="IEventHandler{T}.HandleAsync"/>
    public async Task HandleAsync(ReaderCreatedEvent message, CancellationToken cancellationToken)
    {
        await _activityLogRepository.Add(message.ToActivityLog(), readerId: message.ReaderId);
    }
}