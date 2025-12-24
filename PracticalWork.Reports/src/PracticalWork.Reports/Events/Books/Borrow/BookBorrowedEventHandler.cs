using PracticalWork.Reports.Abstractions.Storage;
using PracticalWork.Reports.SharedKernel.Abstractions;

namespace PracticalWork.Reports.Events.Books.Borrow;

public class BookBorrowedEventHandler: IEventHandler<BookBorrowedEvent>
{
    private readonly IActivityLogRepository _activityLogRepository;

    public BookBorrowedEventHandler(IActivityLogRepository activityLogRepository)
    {
        _activityLogRepository = activityLogRepository;
    }
    
    public async Task HandleAsync(BookBorrowedEvent message, CancellationToken cancellationToken)
    {
        await _activityLogRepository.Add(message.ToActivityLog(), bookId: message.BookId, readerId: message.ReaderId);
    }
}