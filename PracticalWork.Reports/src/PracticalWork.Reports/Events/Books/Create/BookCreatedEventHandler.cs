using Microsoft.Extensions.Logging;
using PracticalWork.Reports.Abstractions.Storage;
using PracticalWork.Reports.SharedKernel.Abstractions;

namespace PracticalWork.Reports.Events.Books.Create;

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
        await _activityLogRepository.Add(message.ToActivityLog(), bookId: message.BookId);
    }
}