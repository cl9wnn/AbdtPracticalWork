namespace PracticalWork.Library.BackgroundJobs.Quartz.Options;

public class JobSettings
{
    /// <summary>
    /// Словарь конфигураций фоновых задач системы
    /// </summary>
    public Dictionary<string, JobConfiguration> Jobs { get; set; } = new();
}
