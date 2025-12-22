using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PracticalWork.Reports.SharedKernel.Abstractions;
using PracticalWork.Reports.SharedKernel.Events;

namespace PracticalWork.Reports.MessageBroker.Kafka;

public class KafkaConsumer<TEvent> : BackgroundService where TEvent : BaseEvent
{
    private readonly string _topic;
    private readonly IConsumer<string, TEvent> _consumer;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<KafkaConsumer<TEvent>> _logger;

    public KafkaConsumer(IOptions<KafkaOptions> options,
        ILogger<KafkaConsumer<TEvent>> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;

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

                    using var scope = _scopeFactory.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<IEventHandler<TEvent>>();

                    await handler.HandleAsync(result.Message.Value, stoppingToken);
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