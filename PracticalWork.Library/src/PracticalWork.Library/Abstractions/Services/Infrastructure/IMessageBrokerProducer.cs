using PracticalWork.Library.SharedKernel.Events;

namespace PracticalWork.Library.Abstractions.Services.Infrastructure;

public interface IMessageBrokerProducer : IDisposable
{
    Task ProduceAsync<TEvent>(string key, TEvent message, CancellationToken cancellationToken = default)
        where TEvent : BaseEvent;
}