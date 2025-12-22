using Microsoft.Extensions.Logging;
using PracticalWork.Reports.Events.Books.Create;
using PracticalWork.Reports.SharedKernel.Abstractions;

namespace PracticalWork.Reports.Events.Books.Archive;

public class BookArchivedEventHandler : IEventHandler<BookArchivedEvent>
{
    private readonly ILogger<BookArchivedEventHandler> _logger;

    public BookArchivedEventHandler(ILogger<BookArchivedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(BookArchivedEvent message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BookArchivedEvent: {title}, {type}, {id}", message.Title, message.EventType,
            message.ArchivedAt);
        
        return Task.CompletedTask;
    }
}