using Microsoft.Extensions.Logging;
using PracticalWork.Reports.SharedKernel.Abstractions;

namespace PracticalWork.Reports.Events.Books.Create;

public class BookCreatedEventHandler : IEventHandler<BookCreatedEvent>
{
    private readonly ILogger<BookCreatedEventHandler> _logger;

    public BookCreatedEventHandler(ILogger<BookCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(BookCreatedEvent message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BookCreatedEvent: {title}, {type}, {year}", message.Title, message.EventType,
            message.Year);
        return Task.CompletedTask;
    }
}