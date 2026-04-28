using Microsoft.Extensions.Options;
using PracticalWork.Reports.Abstractions.Services.Domain;
using PracticalWork.Reports.Abstractions.Services.Infrastructure;
using PracticalWork.Reports.Abstractions.Storage;
using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Enums;
using PracticalWork.Reports.Exceptions;
using PracticalWork.Reports.Models;
using PracticalWork.Reports.Options;
using PracticalWork.Reports.Options.Cache;

namespace PracticalWork.Reports.Services;

/// <summary>
/// Сервис по работе с отчетами
/// </summary>
public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    private readonly IActivityLogRepository _activityLogRepository;
    private readonly ITabularCsvExportService<ActivityLog> _tabularCsvExportService;
    private readonly IKeyValueCsvExportService<WeeklyStatisticsDto> _keyValueCsvExportService;
    private readonly IFileStorageService _fileStorageService;
    private readonly ICacheService _cacheService;
    private readonly IOptions<BooksCacheOptions> _cacheOptions;
    private readonly string _reportsCacheVersionPrefix;

    public ReportService(IReportRepository reportRepository, ITabularCsvExportService<ActivityLog> tabularCsvExportService,
        IFileStorageService fileStorageService, IActivityLogRepository activityLogRepository,
        ICacheService cacheService,
        IOptions<BooksCacheOptions> cacheOptions,
        IKeyValueCsvExportService<WeeklyStatisticsDto> keyValueCsvExportService)
    {
        _reportRepository = reportRepository;
        _tabularCsvExportService = tabularCsvExportService;
        _fileStorageService = fileStorageService;
        _activityLogRepository = activityLogRepository;
        _cacheService = cacheService;
        _cacheOptions = cacheOptions;
        _keyValueCsvExportService = keyValueCsvExportService;

        _reportsCacheVersionPrefix = cacheOptions.Value.ReportsCacheVersionPrefix;
    }

    /// <inheritdoc cref="IReportService.GenerateActivityLogsReport"/>
    public async Task<Report> GenerateActivityLogsReport(GenerateActivityLogReportDto dto)
    {
        var logs = await _activityLogRepository.GetActivityLogsByPeriod(dto.PeriodFrom,
            dto.PeriodTo, dto.EventType);

        var csvBytes = _tabularCsvExportService.Generate(logs);
        var fileName = $"report_{dto.EventType}_{dto.PeriodFrom:yyyyMMdd}-{dto.PeriodTo:yyyyMMdd}";
        
        return await GenerateReport(csvBytes, fileName, dto.PeriodFrom, dto.PeriodTo);
    }

    /// <inheritdoc cref="IReportService.GenerateWeeklyStatisticsReport"/>
    public async Task<Report> GenerateWeeklyStatisticsReport(GenerateWeeklyReportDto dto)
    {
        var csvBytes = _keyValueCsvExportService.Generate(dto.WeeklyStatistics);
        var fileName = $"weekly_report_{dto.PeriodFrom:yyyyMMdd}-{dto.PeriodTo:yyyyMMdd}";
       
        return await GenerateReport(csvBytes, fileName, dto.PeriodFrom, dto.PeriodTo);
    }

    /// <inheritdoc cref="IReportService.GetAll"/>
    public async Task<IReadOnlyCollection<Report>> GetAll()
    {
        var keyPrefix = _cacheOptions.Value.ReportsListCache.KeyPrefix;
        var ttlMinutes = _cacheOptions.Value.ReportsListCache.TtlMinutes;

        var cachedReports = await _cacheService.GetAsync<IReadOnlyList<Report>>(keyPrefix, _reportsCacheVersionPrefix);

        if (cachedReports != null)
        {
            return cachedReports;
        }

        var reports = await _reportRepository.GetAll();
        await _cacheService.SetAsync(keyPrefix, _reportsCacheVersionPrefix, reports,
            TimeSpan.FromMinutes(ttlMinutes));

        return reports;
    }

    /// <inheritdoc cref="IReportService.GetDownloadUrl"/>
    public async Task<string> GetDownloadUrl(string reportName)
    {
        var report = await _reportRepository.GetByName(reportName);
        
        return await _fileStorageService.GetFilePathAsync(report.FilePath);
    }
    
    private async Task<Report> GenerateReport(
        byte[] csvBytes,
        string fileName,
        DateOnly periodFrom,
        DateOnly periodTo)
    {
        var filePath = $"{DateTime.Today.Year}/{DateTime.Today.Month}/{fileName}.csv";

        await _fileStorageService.UploadFileAsync(filePath, csvBytes, "text/csv");

        var report = new Report
        {
            Name = fileName,
            FilePath = filePath,
            Status = ReportStatus.Generated,
            GeneratedAt = DateTime.UtcNow,
            PeriodFrom = periodFrom,
            PeriodTo = periodTo,
        };

        await _reportRepository.Add(report);
        await _cacheService.IncrementVersionAsync(_reportsCacheVersionPrefix);

        return report;
    }
}