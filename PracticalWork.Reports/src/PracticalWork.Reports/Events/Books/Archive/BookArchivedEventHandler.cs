using Microsoft.Extensions.Logging;
using PracticalWork.Reports.Abstractions.Storage;
using PracticalWork.Reports.Events.Books.Create;
using PracticalWork.Reports.SharedKernel.Abstractions;

namespace PracticalWork.Reports.Events.Books.Archive;

public class BookArchivedEventHandler : IEventHandler<BookArchivedEvent>
{
    private readonly IActivityLogRepository _activityLogRepository;

    public BookArchivedEventHandler(IActivityLogRepository activityLogRepository)
    {
        _activityLogRepository = activityLogRepository;
    }

    public async Task HandleAsync(BookArchivedEvent message, CancellationToken cancellationToken)
    {
        await _activityLogRepository.Add(message.ToActivityLog(), bookId: message.BookId);
    }
}