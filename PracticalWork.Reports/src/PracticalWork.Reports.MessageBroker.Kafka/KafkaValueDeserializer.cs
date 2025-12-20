using System.Text.Json;
using Confluent.Kafka;

namespace PracticalWork.Reports.MessageBroker.Kafka;

public class KafkaValueDeserializer<TEvent>: IDeserializer<TEvent>
{
    public TEvent Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        return JsonSerializer.Deserialize<TEvent>(data)!;
    }
}