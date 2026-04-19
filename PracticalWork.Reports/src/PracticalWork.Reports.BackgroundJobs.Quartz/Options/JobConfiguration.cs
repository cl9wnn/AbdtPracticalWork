namespace PracticalWork.Reports.BackgroundJobs.Quartz.Options;

/// <summary>
/// Конфигурация отдельной фоновой задачи системы библиотеки
/// </summary>
public class JobConfiguration
{
    /// <summary>
    /// Cron выражение для планирования регулярного выполнения задачи
    /// </summary>
    public string CronExpression { get; set; } = null!;

    /// <summary>
    /// Максимальное количество повторных попыток выполнения при ошибке
    /// </summary>
    public int MaxRetries { get; set; }
    
    /// <summary>
    /// Максимальное время выполнения задачи в минутах до принудительного прерывания
    /// </summary>
    public int TimeoutMinutes { get; set; }
}