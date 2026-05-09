using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using PracticalWork.Shared.Abstractions.Events;

namespace PracticalWork.Reports.MessageBroker.Kafka;

/// <summary>
/// Десериализатор для преобразования байтового массива из Kafka в объекты событий
/// </summary>
public sealed class KafkaValueDeserializer : IDeserializer<BaseEvent>
{
    private readonly IEventTypeRegistry _registry;

    public KafkaValueDeserializer(IEventTypeRegistry registry)
    {
        _registry = registry;
    }

    /// <summary>
    /// Десериализует данные из брокера сообщений в соответствующий тип события
    /// </summary>
    /// <param name="data">Байтовые данные сообщения, полученные от брокера</param>
    /// <param name="isNull">Указывает, является ли сообщение null</param>
    /// <param name="context">Контекст сериализации, содержащий информацию о топике/партиции</param>
    /// <returns>Десериализованный объект события, унаследованный от <see cref="BaseEvent"/></returns>
    public BaseEvent Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull || data.IsEmpty)
            return null!;

        var json = Encoding.UTF8.GetString(data);

        using var doc = JsonDocument.Parse(json);
        var eventType = doc.RootElement.GetProperty("EventType").GetString()!;

        var clrType = _registry.GetEventType(eventType);

        return (BaseEvent)JsonSerializer.Deserialize(json, clrType)!;
    }
}
