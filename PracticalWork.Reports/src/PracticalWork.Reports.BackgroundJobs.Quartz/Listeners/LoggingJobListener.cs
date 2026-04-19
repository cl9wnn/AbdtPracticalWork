using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Listener;

namespace PracticalWork.Reports.BackgroundJobs.Quartz.Listeners;

/// <summary>
/// Глобальный <see cref="IJobListener"/> для логирования жизненного цикла Quartz-задач
/// </summary>
/// <remarks>
/// Логирует:
/// <list type="bullet">
/// <item><description>Старт выполнения задачи</description></item>
/// <item><description>Успешное завершение</description></item>
/// <item><description>Ошибки выполнения</description></item>
/// </list>
/// 
/// Listener применяется ко всем Job'ам, зарегистрированным в Quartz Scheduler.
/// </remarks>
public class LoggingJobListener : JobListenerSupport
{
    private readonly ILogger<LoggingJobListener> _logger;

    public LoggingJobListener(ILogger<LoggingJobListener> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Уникальное имя listener'а.
    /// </summary>
    public override string Name => "GlobalLoggingJobListener";

    /// <summary>
    /// Вызывается перед выполнением Job
    /// </summary>
    /// <param name="context">Контекст выполнения задачи.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public override Task JobToBeExecuted(
        IJobExecutionContext context,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Job START: {JobName} | TimeUtc: {FireTimeUtc}",
            context.JobDetail.Key.Name,
            context.FireTimeUtc.UtcDateTime);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Вызывается после выполнения Job (как при успехе, так и при ошибке)
    /// </summary>
    /// <param name="context">Контекст выполнения задачи.</param>
    /// <param name="jobException">Исключение, если выполнение завершилось с ошибкой.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    public override Task JobWasExecuted(
        IJobExecutionContext context,
        JobExecutionException? jobException,
        CancellationToken cancellationToken = default)
    {
        if (jobException != null)
        {
            _logger.LogError(
                jobException,
                "Job FAILED: {JobName}",
                context.JobDetail.Key.Name);
        }
        else
        {
            _logger.LogInformation(
                "Job END: {JobName} | RunTime: {RunTime}",
                context.JobDetail.Key.Name,
                context.JobRunTime);
        }

        return Task.CompletedTask;
    }
}