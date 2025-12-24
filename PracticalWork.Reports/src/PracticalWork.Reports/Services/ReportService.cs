using Microsoft.Extensions.Options;
using PracticalWork.Reports.Abstractions.Services.Domain;
using PracticalWork.Reports.Abstractions.Services.Infrastructure;
using PracticalWork.Reports.Abstractions.Storage;
using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Enums;
using PracticalWork.Reports.Exceptions;
using PracticalWork.Reports.Models;
using PracticalWork.Reports.Options;

namespace PracticalWork.Reports.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    private readonly IActivityLogRepository _activityLogRepository;
    private readonly ICsvExportService<ActivityLog> _csvExportService;
    private readonly IFileStorageService _fileStorageService;
    private readonly ICacheService _cacheService;
    private IOptions<BooksCacheOptions> _cacheOptions;
    private readonly string _reportsCacheVersionPrefix;

    public ReportService(IReportRepository reportRepository, ICsvExportService<ActivityLog> csvExportService,
        IFileStorageService fileStorageService, IActivityLogRepository activityLogRepository,
        ICacheService cacheService,
        IOptions<BooksCacheOptions> options, IOptions<BooksCacheOptions> cacheOptions)
    {
        _reportRepository = reportRepository;
        _csvExportService = csvExportService;
        _fileStorageService = fileStorageService;
        _activityLogRepository = activityLogRepository;
        _cacheService = cacheService;
        _cacheOptions = cacheOptions;

        _reportsCacheVersionPrefix = options.Value.ReportsCacheVersionPrefix;
    }

    public async Task<Report> Generate(GenerateReportDto dto)
    {
        var logs = await _activityLogRepository.GetActivityLogsByPeriodAsync(dto.PeriodFrom,
            dto.PeriodTo, dto.EventType);

        var csvBytes = _csvExportService.Generate(logs);

        var fileName = $"report_{dto.EventType}_{dto.PeriodFrom:yyyyMMdd}-{dto.PeriodTo:yyyyMMdd}";
        var filePath = $"{DateTime.Today.Year}/{DateTime.Today.Month}/{fileName}.csv";

        await _fileStorageService.UploadFileAsync(filePath, csvBytes, "text/csv");

        var report = new Report
        {
            Name = fileName,
            FilePath = filePath,
            Status = ReportStatus.Generated,
            GeneratedAt = DateTime.UtcNow,
            PeriodFrom = dto.PeriodFrom,
            PeriodTo = dto.PeriodTo,
        };

        await _reportRepository.Add(report);
        await _cacheService.IncrementVersionAsync(_reportsCacheVersionPrefix);

        return report;
    }

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

    public async Task<string> GetDownloadUrl(string reportName)
    {
        var report = await _reportRepository.GetByName(reportName);
        
        return await _fileStorageService.GetFilePathAsync(report.FilePath);
    }
}