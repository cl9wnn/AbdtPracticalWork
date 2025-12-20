namespace PracticalWork.Reports.MessageBroker.Kafka;

public class KafkaOptions
{
    public string BootstrapServers { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public string GroupId { get; set; } = string.Empty;
    public string AutoOffsetReset { get; set; } = string.Empty;
    public bool EnableAutoCommit { get; set; }
}