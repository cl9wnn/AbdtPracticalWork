using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PracticalWork.Reports.Abstractions.Services.Domain;
using PracticalWork.Reports.Contracts.v1.Abstracts;
using PracticalWork.Reports.Contracts.v1.ActivityLogs.Get;
using PracticalWork.Reports.Contracts.v1.Reports.Download;
using PracticalWork.Reports.Contracts.v1.Reports.Generate;
using PracticalWork.Reports.Contracts.v1.Reports.Get;
using PracticalWork.Reports.Controllers.Mappers.v1;

namespace PracticalWork.Reports.Controllers.Api.v1;

/// <summary>
/// Контроллер для работы с отчетами
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/reports")]
public class ReportsController: Controller
{
    private readonly IReportService _reportService;
    private readonly IActivityLogService _activityLogService;

    public ReportsController(IReportService reportService, IActivityLogService activityLogService)
    {
        _reportService = reportService;
        _activityLogService = activityLogService;
    }
    
    /// <summary>
    /// Получение логов активности
    /// </summary>
    /// <param name="request">Запрос на получение логов активности, содержащий параметры фильтрации и пагинации</param>
    /// <returns>Страница с логами активности</returns>
    [HttpGet("activity")]
    [ProducesResponseType(typeof(PagedResponse<GetActivityLogsResponse>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetActivityLogs([FromQuery] GetActivityLogsRequest request)
    {
        var result = await _activityLogService.GetPagedActivityLogs(
            request.ToActivityLogFilterDto(),
            request.ToActivityLogPaginationDto());
        
        return Ok(new PagedResponse<GetActivityLogsResponse>(
            result.Page,
            result.PageSize,
            result.Items.ToActivityLogsResponseList()));
    }
    
    /// <summary>
    /// Генерация отчета в формате CSV
    /// </summary>
    /// <param name="request">Запрос на генерацию отчета</param>
    /// <returns>Метаданные сгенерированного отчета</returns>
    [HttpPost("generate")]
    [ProducesResponseType(typeof(GenerateReportResponse), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GenerateCsvReport([FromQuery] GenerateReportRequest request)
    {
        var generatedReport = await _reportService.Generate(request.ToGenerateReportDto());
        return Ok(generatedReport.ToGenerateReportResponse());
    }
    
    /// <summary>
    /// Получение списка отчетов
    /// </summary>
    /// <returns>Список отчетов</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<GetReportResponse>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetReports()
    {
        var reports = await _reportService.GetAll();
        return Ok(reports.ToGetReportResponseList());
    }
    
    /// <summary>
    /// Получение signed URL для скачивания отчета
    /// </summary>
    /// <param name="reportName">Наименование отчета</param>
    /// <returns>Signed URL на скачивание отчета</returns>
    [HttpGet("{reportName}/download")]
    [ProducesResponseType(typeof(DownloadReportResponse), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DownloadReport(string reportName)
    {
        var url = await _reportService.GetDownloadUrl(reportName);
        return Ok(new DownloadReportResponse(url));
    }
}