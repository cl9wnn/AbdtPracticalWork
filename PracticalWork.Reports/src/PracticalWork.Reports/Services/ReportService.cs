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
    private readonly IActivityLogService _activityLogService;
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
        IKeyValueCsvExportService<WeeklyStatisticsDto> keyValueCsvExportService,
        IActivityLogService activityLogService)
    {
        _reportRepository = reportRepository;
        _tabularCsvExportService = tabularCsvExportService;
        _fileStorageService = fileStorageService;
        _activityLogRepository = activityLogRepository;
        _cacheService = cacheService;
        _cacheOptions = cacheOptions;
        _keyValueCsvExportService = keyValueCsvExportService;
        _activityLogService = activityLogService;

        _reportsCacheVersionPrefix = cacheOptions.Value.ReportsCacheVersionPrefix;
    }

    /// <inheritdoc cref="IReportService.GenerateLibraryActivityReport"/>
    public async Task<Report> GenerateLibraryActivityReport(GenerateLibraryActivityReportDto dto, 
        CancellationToken cancellationToken)
    {
        var logs = await _activityLogRepository.GetActivityLogsByPeriod(dto.PeriodFrom,
            dto.PeriodTo, dto.EventType, cancellationToken);

        var csvBytes = _tabularCsvExportService.Generate(logs);
        var fileName = $"report_{dto.EventType}_{dto.PeriodFrom:yyyyMMdd}-{dto.PeriodTo:yyyyMMdd}";
        
        return await GenerateReport(csvBytes, fileName, dto.PeriodFrom, dto.PeriodTo, cancellationToken);
    }

    /// <inheritdoc cref="IReportService.GenerateWeeklyReport"/>
    public async Task<WeeklyReportDto> GenerateWeeklyReport(GenerateWeeklyReportDto dto, 
        CancellationToken cancellationToken)
    {
        var csvBytes = _keyValueCsvExportService.Generate(dto.WeeklyStatistics);
        var fileName = $"weekly_report_{dto.PeriodFrom:yyyyMMdd}-{dto.PeriodTo:yyyyMMdd}";
       
        var generatedReport = await GenerateReport(csvBytes, fileName, dto.PeriodFrom, dto.PeriodTo, cancellationToken);
        var downloadUrl = await GetDownloadUrl(generatedReport.Name, cancellationToken);

        return new WeeklyReportDto
        {
            Name = generatedReport.Name,
            DownloadUrl = downloadUrl,
            GeneratedAt = generatedReport.GeneratedAt
        };
    }

    /// <inheritdoc cref="IReportService.GetAll"/>
    public async Task<IReadOnlyCollection<Report>> GetAll(CancellationToken cancellationToken)
    {
        var keyPrefix = _cacheOptions.Value.ReportsListCache.KeyPrefix;
        var ttlMinutes = _cacheOptions.Value.ReportsListCache.TtlMinutes;

        var cachedReports = await _cacheService.GetAsync<IReadOnlyList<Report>>(keyPrefix, _reportsCacheVersionPrefix, 
            cancellationToken);

        if (cachedReports != null)
        {
            return cachedReports;
        }

        var reports = await _reportRepository.GetAll(cancellationToken);
        await _cacheService.SetAsync(keyPrefix, _reportsCacheVersionPrefix, reports,
            TimeSpan.FromMinutes(ttlMinutes), cancellationToken);

        return reports;
    }

    /// <inheritdoc cref="IReportService.GetDownloadUrl"/>
    public async Task<string> GetDownloadUrl(string reportName, CancellationToken cancellationToken)
    {
        var report = await _reportRepository.GetByName(reportName, cancellationToken);
        
        return await _fileStorageService.GetFilePathAsync(report.FilePath, cancellationToken);
    }
    
    private async Task<Report> GenerateReport(
        byte[] csvBytes,
        string fileName,
        DateOnly periodFrom,
        DateOnly periodTo,
        CancellationToken cancellationToken)
    {
        var filePath = $"{DateTime.Today.Year}/{DateTime.Today.Month}/{fileName}.csv";

        await _fileStorageService.UploadFileAsync(filePath, csvBytes, "text/csv", cancellationToken);

        var report = new Report
        {
            Name = fileName,
            FilePath = filePath,
            Status = ReportStatus.Generated,
            GeneratedAt = DateTime.UtcNow,
            PeriodFrom = periodFrom,
            PeriodTo = periodTo,
        };

        await _reportRepository.Add(report, cancellationToken);
        await _cacheService.IncrementVersionAsync(_reportsCacheVersionPrefix, cancellationToken);

        return report;
    }
}