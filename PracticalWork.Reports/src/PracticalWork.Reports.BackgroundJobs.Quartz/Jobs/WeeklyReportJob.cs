using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PracticalWork.Reports.Abstractions.Services.Domain;
using PracticalWork.Reports.Abstractions.Services.Infrastructure;
using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Options.Email;
using Quartz;

namespace PracticalWork.Reports.BackgroundJobs.Quartz.Jobs;

/// <summary>
/// Фоновая задача, создающая еженедельный отчет со статистикой для администрации
/// </summary>
/// <remarks>Сбор данных происходит за предыдущую неделю (с понедельника по воскресенье)</remarks>
public class WeeklyReportJob : BaseJob
{
    private readonly ILogger<WeeklyReportJob> _logger;
    private readonly IActivityLogService _activityLogService;
    private readonly IReportService _reportService;
    private readonly IEmailService _emailService;
    private readonly WeeklyReportTemplate _weeklyReportTemplate;

    public WeeklyReportJob(
        ILogger<WeeklyReportJob> logger, 
        IActivityLogService activityLogService, 
        IEmailService emailService, 
        IOptions<EmailTemplateSettings> emailTemplateSettings,
        IReportService reportService)
    {
        _logger = logger;
        _activityLogService = activityLogService;
        _emailService = emailService;
        _reportService = reportService;
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
            var statistics = await _activityLogService.GetWeeklyStatistics(from, to, ct);
            
            var report = await _reportService.GenerateWeeklyStatisticsReport(new GenerateWeeklyReportDto
            {
                PeriodFrom = from,
                PeriodTo = to,
                WeeklyStatistics = statistics
            }, ct);
            
            var downloadUrl = await _reportService.GetDownloadUrl(report.Name, ct);
            
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
                GeneratedAt = DateTime.UtcNow,
            };
            
            await _emailService.SendBulkAsync(_weeklyReportTemplate.AdminEmails, messageSubject, message, ct);

            _logger.LogInformation("Еженедельный отчет сгенерирован. " +
                                   "Письма со статистикой успешно отправлены на почты администраторов.");
        }
        catch (Exception ex)
        {
            _logger.LogError("Произошла ошибка при генерации отчета статистикой: {exception}", ex.Message);
        }
    }
}