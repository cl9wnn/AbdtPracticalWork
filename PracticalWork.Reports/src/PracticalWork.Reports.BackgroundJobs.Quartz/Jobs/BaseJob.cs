using PracticalWork.Reports.BackgroundJobs.Quartz.Interfaces;
using Quartz;

namespace PracticalWork.Reports.BackgroundJobs.Quartz.Jobs;

/// <summary>
/// Базовый класс для фоновых задач, реализующий логику таймаутов и повторных попыток для задач
/// </summary>
public abstract class BaseJob : ILibraryJob
{
    /// <inheritdoc cref="ILibraryJob.JobName"/>
    public abstract string JobName { get; }
    
    /// <inheritdoc cref="ILibraryJob.Description"/>
    public abstract string Description { get; }
    
    /// <summary>
    /// Выполнение фоновой задачи по архивации книг
    /// </summary>
    /// <param name="context">Контекст выполнения фоновой задачи</param>
    /// <param name="ct">Токен отмены операции</param>
    protected abstract Task ExecuteJob(IJobExecutionContext context, CancellationToken ct);

    /// <inheritdoc cref="IJob.Execute"/>
    public async Task Execute(IJobExecutionContext context)
    {
        var maxRetries = context.MergedJobDataMap.GetInt("MaxRetries");
        var timeout = context.MergedJobDataMap.GetInt("TimeoutMinutes");
        var retry = context.RefireCount + 1;

        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(timeout));
            await ExecuteJob(context, cts.Token);
        }
        catch (Exception ex)
        {
            if (retry < maxRetries)
            {
                throw new JobExecutionException(ex)
                {
                    RefireImmediately = true
                };
            }

            throw;
        }
    }
}