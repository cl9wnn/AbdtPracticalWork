using PracticalWork.Reports.SharedKernel.Events;

namespace PracticalWork.Reports.SharedKernel.Abstractions;

public interface IEventHandler<in TEvent> where TEvent: BaseEvent
{
    Task HandleAsync(TEvent message, CancellationToken cancellationToken);
}