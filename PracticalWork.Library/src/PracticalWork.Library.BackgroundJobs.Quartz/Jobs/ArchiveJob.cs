using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PracticalWork.Library.Abstractions.Services.Domain;
using PracticalWork.Library.BackgroundJobs.Quartz.Interfaces;
using PracticalWork.Library.Options.Archive;
using Quartz;

namespace PracticalWork.Library.BackgroundJobs.Quartz.Jobs;

/// <summary>
/// Фоновая задача для автоматической архивации старых книг
/// </summary>
/// <remarks>Выполняется 1-го числа каждого месяца в 3:00 по МСК</remarks>
public class ArchiveJob : ILibraryJob
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

    /// <summary>
    /// Уникальное имя задачи
    /// </summary>
    public string JobName { get; } = "Archive Job";

    /// <summary>
    /// Описание задачи для отображения в интерфейсе управления
    /// </summary>
    public string Description { get; } = "Задача для автоматической архивации старых книг." +
                                         "Начинает выполнение 1-го числа каждого месяца в 3:00 по МСК";
    
    /// <summary>
    /// Выполнение фоновой задачи по архивации книг
    /// </summary>
    /// <param name="context">Контекст выполнения фоновой задачи</param>
    public async Task Execute(IJobExecutionContext context)
    {
        var archiveSettings = _archiveSettings.Value;

        var report = await _bookService.ArchiveOldBooks(
            archiveSettings.YearsWithoutBorrow,
            archiveSettings.MaxBooksPerRun);

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