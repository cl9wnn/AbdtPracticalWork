namespace PracticalWork.Library.Abstractions.Services.Infrastructure;

public interface IMessageBrokerProducer<in TMessage>: IDisposable
{
    Task ProduceAsync(TMessage message, string key = null, CancellationToken cancellationToken = default);
}