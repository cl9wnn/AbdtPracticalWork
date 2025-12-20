using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PracticalWork.Reports.SharedKernel.Abstractions;

namespace PracticalWork.Reports.MessageBroker.Kafka;

public class KafkaConsumer<TEvent> : BackgroundService 
{
    private readonly string _topic;
    private readonly IConsumer<string, TEvent> _consumer;
    private readonly IEventHandler<TEvent> _eventHandler;
    private readonly ILogger<KafkaConsumer<TEvent>> _logger;

    public KafkaConsumer(IOptions<KafkaOptions> options, IEventHandler<TEvent> eventHandler,
        ILogger<KafkaConsumer<TEvent>> logger)
    {
        _eventHandler = eventHandler;
        _logger = logger;
        
        var config = new ConsumerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
            GroupId = options.Value.GroupId,
            AutoOffsetReset = Enum.Parse<AutoOffsetReset>(options.Value.AutoOffsetReset, true),
            EnableAutoCommit = options.Value.EnableAutoCommit,
        };

        _topic = options.Value.Topic;

        _consumer = new ConsumerBuilder<string, TEvent>(config)
            .SetValueDeserializer(new KafkaValueDeserializer<TEvent>())
            .Build();
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
        => ConsumeAsync(stoppingToken);

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer.Close();
        _consumer.Dispose();
        return base.StopAsync(cancellationToken);
    }
    
    private async Task ConsumeAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(stoppingToken);
                    if (result?.Message == null)
                        continue;
                    
                    await _eventHandler.HandleAsync(result.Message.Value, stoppingToken);
                    _consumer.Commit(result);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Kafka consume error: {Reason}", ex.Error.Reason);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while processing event message");
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Kafka consumer stopping...");
        }
    }
}