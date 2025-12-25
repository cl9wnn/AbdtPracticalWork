using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PracticalWork.Reports.SharedKernel.Abstractions;
using PracticalWork.Reports.SharedKernel.Events;

namespace PracticalWork.Reports.MessageBroker.Kafka;

public sealed class KafkaConsumer : BackgroundService
{
    private readonly IConsumer<string, BaseEvent> _consumer;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<KafkaConsumer> _logger;
    private readonly string _topic;

    public KafkaConsumer(
        IOptions<KafkaOptions> options,
        IServiceScopeFactory scopeFactory,
        ILogger<KafkaConsumer> logger,
        IEventTypeRegistry registry)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _topic = options.Value.Topic;

        var config = new ConsumerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
            GroupId = options.Value.GroupId,
            AutoOffsetReset = Enum.Parse<AutoOffsetReset>(options.Value.AutoOffsetReset, true),
            EnableAutoCommit = options.Value.EnableAutoCommit,
        };

        _consumer = new ConsumerBuilder<string, BaseEvent>(config)
            .SetValueDeserializer(new KafkaValueDeserializer(registry))
            .Build();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_topic);

        return Task.Run(async () =>
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = _consumer.Consume(stoppingToken);
                        if (result?.Message?.Value == null)
                            continue;

                        await DispatchAsync(result.Message.Value, stoppingToken);
                        _consumer.Commit(result);
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError(ex, "Kafka consume error: {Reason}", ex.Error.Reason);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error while processing event");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Kafka consumer stopping...");
            }
        }, stoppingToken);
    }

    private async Task DispatchAsync(BaseEvent @event, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var provider = scope.ServiceProvider;

        var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
        var handler = provider.GetRequiredService(handlerType);

        await (Task)handlerType
            .GetMethod(nameof(IEventHandler<BaseEvent>.HandleAsync))!
            .Invoke(handler, [@event, ct])!;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer.Close();
        _consumer.Dispose();
        return base.StopAsync(cancellationToken);
    }
}
