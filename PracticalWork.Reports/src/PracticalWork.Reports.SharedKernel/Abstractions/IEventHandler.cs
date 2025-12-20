namespace PracticalWork.Reports.SharedKernel.Abstractions;

public interface IEventHandler<in TEvent>
{
    Task HandleAsync(TEvent message, CancellationToken cancellationToken);
}