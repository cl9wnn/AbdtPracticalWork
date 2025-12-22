using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PracticalWork.Library.Abstractions.Services.Infrastructure;
using PracticalWork.Library.SharedKernel.Events;

namespace PracticalWork.Library.MessageBroker.Kafka;

public class KafkaProducer : IMessageBrokerProducer
{
    private readonly string _topic;
    private readonly IProducer<string, byte[]> _producer;
    private readonly ILogger<KafkaProducer> _logger;

    public KafkaProducer(IOptions<KafkaOptions> options, ILogger<KafkaProducer> logger)
    {
        _logger = logger;
        var config = new ProducerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
        };

        _producer = new ProducerBuilder<string, byte[]>(config)
            .SetErrorHandler((_, e) => _logger.LogError("Kafka Producer error: {Reason}", e.Reason))
            .Build();

        _topic = options.Value.Topic;
    }

    public async Task ProduceAsync<TEvent>(string key, TEvent message, CancellationToken cancellationToken = default)
        where TEvent : BaseEvent
    {
        var kafkaMessage = new Message<string, byte[]>
        {
            Key = key,
            Value = JsonSerializer.SerializeToUtf8Bytes(message)
        };

        try
        {
            await _producer.ProduceAsync(_topic, kafkaMessage, cancellationToken);
        }
        catch (ProduceException<string, byte[]> ex)
        {
            _logger.LogError(ex, "Failed to produce message: {Msg}", ex.Error.Reason);
            throw;
        }
    }

    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(10));
        _producer.Dispose();
    }
}