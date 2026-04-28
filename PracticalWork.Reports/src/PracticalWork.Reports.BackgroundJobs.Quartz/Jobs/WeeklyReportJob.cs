using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PracticalWork.Reports.Abstractions.Services.Domain;
using PracticalWork.Reports.Abstractions.Services.Infrastructure;
using PracticalWork.Reports.Abstractions.Storage;
using PracticalWork.Reports.BackgroundJobs.Quartz.Interfaces;
using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Enums;
using PracticalWork.Reports.Options.Email;
using Quartz;

namespace PracticalWork.Reports.BackgroundJobs.Quartz.Jobs;

/// <summary>
/// Фоновая задача, создающая еженедельный отчет со статистикой для администрации
/// </summary>
/// <remarks>Сбор данных происходит за предыдущую неделю (с понедельника по воскресенье)</remarks>
public class WeeklyReportJob : ILibraryJob
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
    
    /// <summary>
    /// Уникальное имя задачи
    /// </summary>
    public string JobName { get; } = "Weekly Report Job";
    
    /// <summary>
    /// Описание задачи для отображения в интерфейсе управления
    /// </summary>
    public string Description { get; } = "Задача, создающая еженедельный отчет со статистикой для администрации";
    
    /// <summary>
    /// Выполнение фоновой задачи
    /// </summary>
    /// <param name="context">Контекст выполнения фоновой задачи</param>
    public async Task Execute(IJobExecutionContext context)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        var from = today.AddDays(-((int)today.DayOfWeek + 6)); // прошлый понедельник
        var to = from.AddDays(6); // прошлое воскресенье

        try
        {
            var statistics = await _activityLogService.GetWeeklyStatistics(from, to);
            
            var report = await _reportService.GenerateWeeklyStatisticsReport(new GenerateWeeklyReportDto
            {
                PeriodFrom = from,
                PeriodTo = to,
                WeeklyStatistics = statistics
            });
            
            var downloadUrl = await _reportService.GetDownloadUrl(report.Name);
            
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
            
            await _emailService.SendBulkAsync(_weeklyReportTemplate.AdminEmails, messageSubject, message);

            _logger.LogInformation("Еженедельный отчет сгенерирован. " +
                                   "Письма со статистикой успешно отправлены на почты администраторов.");
        }
        catch (Exception ex)
        {
            _logger.LogError("Произошла ошибка при генерации отчета статистикой: {exception}", ex.Message);
        }
    }
}