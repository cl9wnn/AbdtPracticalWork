using PracticalWork.Reports.Abstractions.Storage;
using PracticalWork.Reports.Events.Books.Create;
using PracticalWork.Reports.SharedKernel.Abstractions;

namespace PracticalWork.Reports.Events.Books.Return;

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
        await _activityLogRepository.Add(message.ToActivityLog(), bookId: message.BookId, readerId: message.ReaderId);
    }
}