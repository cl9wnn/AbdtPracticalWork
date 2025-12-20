using Microsoft.Extensions.Hosting;

namespace PracticalWork.Reports.MessageBroker.Kafka;

public class KafkaConsumer<TMessage> : BackgroundService 
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}