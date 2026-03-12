using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PracticalWork.Reports.SharedKernel.Abstractions;
using PracticalWork.Reports.SharedKernel.Events;

namespace PracticalWork.Reports.MessageBroker.Kafka;

/// <summary>
/// Сервис-потребитель для получения и обработки сообщений из Kafka
/// </summary>
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

    /// <summary>
    /// Основной цикл обработки сообщений из Kafka.
    /// Подписывается на топик и непрерывно обрабатывает входящие сообщения.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _consumer.Subscribe(_topic);

        return Task.Run(async () =>
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = _consumer.Consume(cancellationToken);
                        if (result?.Message?.Value == null)
                            continue;

                        await DispatchAsync(result.Message.Value, cancellationToken);
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
        }, cancellationToken);
    }

    /// <summary>
    /// Отправляет полученное событие соответствующему обработчику через DI-контейнер.
    /// </summary>
    /// <param name="event">Событие для обработки</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    private async Task DispatchAsync(BaseEvent @event, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var provider = scope.ServiceProvider;

        var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
        var handler = provider.GetRequiredService(handlerType);

        await (Task)handlerType
            .GetMethod(nameof(IEventHandler<BaseEvent>.HandleAsync))!
            .Invoke(handler, [@event, cancellationToken])!;
    }

    /// <summary>
    /// Останавливает потребитель сообщений и освобождает ресурсы.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns></returns>
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer.Close();
        _consumer.Dispose();
        return base.StopAsync(cancellationToken);
    }
}
