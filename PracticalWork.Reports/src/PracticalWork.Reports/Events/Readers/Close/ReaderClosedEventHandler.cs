using PracticalWork.Reports.Abstractions.Storage;
using PracticalWork.Reports.Events.Books.Return;
using PracticalWork.Reports.SharedKernel.Abstractions;

namespace PracticalWork.Reports.Events.Readers.Close;

public class ReaderClosedEventHandler: IEventHandler<ReaderClosedEvent>
{
    private readonly IActivityLogRepository _activityLogRepository;

    public ReaderClosedEventHandler(IActivityLogRepository activityLogRepository)
    {
        _activityLogRepository = activityLogRepository;
    }

    public async Task HandleAsync(ReaderClosedEvent message, CancellationToken cancellationToken)
    {
        await _activityLogRepository.Add(message.ToActivityLog(), readerId: message.ReaderId);
    }
}