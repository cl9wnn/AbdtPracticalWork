using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PracticalWork.Library.Abstractions.Services.Domain;
using PracticalWork.Library.Abstractions.Services.Infrastructure;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Options.Email;
using PracticalWork.Shared.Contracts.Http.Reports.WeeklyReport;
using Quartz;

namespace PracticalWork.Library.BackgroundJobs.Quartz.Jobs;

/// <summary>
/// Фоновая задача, создающая еженедельный отчет со статистикой для администрации
/// </summary>
/// <remarks>Сбор данных происходит за предыдущую неделю (с понедельника по воскресенье)</remarks>
public class WeeklyReportJob : BaseJob
{
    private readonly ILogger<WeeklyReportJob> _logger;
    private readonly ILibraryService _libraryService;
    private readonly IReportsApiClient _reportApiClient;
    private readonly IEmailService _emailService;
    private readonly WeeklyReportTemplate _weeklyReportTemplate;

    public WeeklyReportJob(
        ILogger<WeeklyReportJob> logger,
        ILibraryService activityLogService,
        IEmailService emailService,
        IOptions<EmailTemplateSettings> emailTemplateSettings,
        IReportsApiClient reportApiClient)
    {
        _logger = logger;
        _libraryService = activityLogService;
        _emailService = emailService;
        _reportApiClient = reportApiClient;
        _weeklyReportTemplate = emailTemplateSettings.Value.WeeklyReport;
    }

    /// <inheritdoc cref="BaseJob.JobName"/>
    public override string JobName { get; } = "Weekly Report Job";

    /// <inheritdoc cref="BaseJob.Description"/>
    public override string Description { get; } = "Задача, создающая еженедельный отчет со статистикой для администрации";

    /// <inheritdoc cref="BaseJob.ExecuteJob"/>
    protected override async Task ExecuteJob(IJobExecutionContext context, CancellationToken ct)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        var from = today.AddDays(-((int)today.DayOfWeek + 6)); // прошлый понедельник
        var to = from.AddDays(6); // прошлое воскресенье

        try
        {
            var statistics = await _libraryService.GetLibraryStatistics(from, to, ct);

            var report = await _reportApiClient.GenerateWeeklyReport(new GenerateWeeklyReportRequest(
                PeriodFrom: from,
                PeriodTo: to,
                NewBooksCount: statistics.NewBooksCount,
                NewReadersCount: statistics.NewReadersCount,
                BorrowedBooksCount: statistics.BorrowedBooksCount,
                ReturnedBooksCount: statistics.ReturnedBooksCount,
                OverdueBooksCount: statistics.OverdueBooksCount
            ), ct);

            var downloadUrl = report.DownloadUrl;

            var messageSubject = string.Format(_weeklyReportTemplate.SubjectTemplate,
                from.ToString("dd.MM.yyyy"),
                to.ToString("dd.MM.yyyy"));

            var message = new WeeklyReportEmailMessageDto
            {
                StartDate = from,
                EndDate = to,
                NewBooksCount = statistics.NewBooksCount,
                NewReadersCount = statistics.NewReadersCount,
                BorrowedBooksCount = statistics.BorrowedBooksCount,
                ReturnedBooksCount = statistics.ReturnedBooksCount,
                OverdueBooksCount = statistics.OverdueBooksCount,
                ReportDownloadLink = downloadUrl,
                GeneratedAt = report.GeneratedAt,
            };

            await _emailService.SendBulkAsync(_weeklyReportTemplate.AdminEmails, messageSubject, message, ct);

            _logger.LogInformation("Еженедельный отчет {name} сгенерирован в {time}. " +
                                   "Письма со статистикой успешно отправлены на почты администраторов.",
                report.Name, report.GeneratedAt);
        }
        catch (Exception ex)
        {
            _logger.LogError("Произошла ошибка при генерации отчета статистикой: {exception}", ex.Message);
        }
    }
}