using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PracticalWork.Library.Abstractions.Services.Domain;
using PracticalWork.Library.BackgroundJobs.Quartz.Interfaces;
using PracticalWork.Library.Options.Jobs;
using Quartz;

namespace PracticalWork.Library.BackgroundJobs.Quartz.Jobs;

/// <summary>
/// Фоновая задача для автоматической архивации старых книг
/// </summary>
public class ArchiveJob : BaseJob
{
    private readonly ILogger<ArchiveJob> _logger;
    private readonly IBookService _bookService;
    private readonly IOptions<ArchiveSettings> _archiveSettings;

    public ArchiveJob(ILogger<ArchiveJob> logger, IBookService bookService, IOptions<ArchiveSettings> archiveSettings)
    {
        _logger = logger;
        _bookService = bookService;
        _archiveSettings = archiveSettings;
    }

    /// <inheritdoc cref="BaseJob.JobName"/>
    public override string JobName { get; } = "Archive Job";
    
    /// <inheritdoc cref="BaseJob.Description"/>
    public override string Description { get; } = "Задача для автоматической архивации старых книг.";
    
    /// <inheritdoc cref="BaseJob.ExecuteJob"/>
    protected override async Task ExecuteJob(IJobExecutionContext context, CancellationToken cancellationToken)
    {
        var archiveSettings = _archiveSettings.Value;

        var report = await _bookService.ArchiveOldBooks(
            archiveSettings.YearsWithoutBorrow,
            archiveSettings.MaxBooksPerRun,
            cancellationToken);

        _logger.LogInformation("""
                               Плановая архивация завершена:
                               Всего обработано книг: {Total}
                               Успешно архивировано: {Success}
                               Пропущено: {Skipped}
                               Время выполнения: {Time} ms
                               """,
            report.TotalProcessed,
            report.SuccessfullyArchived,
            report.Skipped,
            report.ExecutionTimeMs);
        
        foreach (var skipped in report.SkippedDetails)
        {
            _logger.LogWarning("Пропущена книга с ID={BookId}. Причина: {Reason}",
                skipped.BookId,
                skipped.Reason);
        }
    }
}