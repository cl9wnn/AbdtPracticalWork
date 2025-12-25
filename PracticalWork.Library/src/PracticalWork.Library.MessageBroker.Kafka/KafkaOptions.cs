namespace PracticalWork.Library.MessageBroker.Kafka;

/// <summary>
/// Настройки для подключения к Apache Kafka
/// </summary>
public class KafkaOptions
{
    /// <summary>
    /// Адреса серверов Kafka (разделенные запятыми, например: "localhost:9092")
    /// </summary>
    public string BootstrapServers { get; set; } = string.Empty;
    
    /// <summary>
    /// Название топика Kafka для отправки/получения сообщений
    /// </summary>
    public string Topic { get; set; } = string.Empty;
}