using PracticalWork.Library.Abstractions.Services.Infrastructure;

namespace PracticalWork.Library.MessageBroker.Kafka;

public class KafkaProducer<TMessage> : IMessageBrokerProducer<TMessage>
{
    public Task ProduceAsync(TMessage message, string? key = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}
