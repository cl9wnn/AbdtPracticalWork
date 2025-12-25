namespace PracticalWork.Reports.MessageBroker.Kafka;

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
    
    /// <summary>
    /// Идентификатор группы потребителей (Consumer Group)
    /// </summary>
    public string GroupId { get; set; } = string.Empty;
    
    /// <summary>
    /// Стратегия обработки отсутствующего смещения (earliest/latest/none)
    /// </summary>
    public string AutoOffsetReset { get; set; } = string.Empty;
    
    /// <summary>
    /// Автоматическое подтверждение получения сообщений
    /// </summary>
    public bool EnableAutoCommit { get; set; }
}