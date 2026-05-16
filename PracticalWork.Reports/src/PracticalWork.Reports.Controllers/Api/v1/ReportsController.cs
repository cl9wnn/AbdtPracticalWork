using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PracticalWork.Reports.Abstractions.Services.Domain;
using PracticalWork.Reports.Contracts.v1.Reports.Download;
using PracticalWork.Reports.Contracts.v1.Reports.Generate;
using PracticalWork.Reports.Contracts.v1.Reports.Get;
using PracticalWork.Reports.Controllers.Mappers.v1;
using PracticalWork.Shared.Contracts.Http.Reports.WeeklyReport;

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

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }
    
    /// <summary>
    /// Генерация отчета в формате CSV с данными об активности библиотеки
    /// </summary>
    /// <param name="request">Запрос на генерацию отчета</param>
    /// <returns>Метаданные сгенерированного отчета</returns>
    [HttpPost("generate-activity")]
    [ProducesResponseType(typeof(GenerateLibraryActivityReportResponse), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GenerateLibraryActivityReport([FromQuery] GenerateLibraryActivityReportRequest request)
    {
        var generatedReport = await _reportService.GenerateLibraryActivityReport(request.ToGenerateLibraryActivityReportDto());
        return Ok(generatedReport.ToGenerateLibraryActivityReportResponse());
    }
    
    /// <summary>
    /// Генерация отчета в формате CSV с еженедельной статистикой библиотеки
    /// </summary>
    /// <param name="request">Запрос на генерацию отчета</param>
    /// <returns>Signed URL на скачивание отчета</returns>
    [HttpPost("generate-weekly")]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GenerateWeeklyReport([FromBody] GenerateWeeklyReportRequest request)
    {
        var generatedReport = await _reportService.GenerateWeeklyReport(request.ToGenerateWeeklyReportDto());
        return Ok(generatedReport.ToGenerateWeeklyReportResponse());
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