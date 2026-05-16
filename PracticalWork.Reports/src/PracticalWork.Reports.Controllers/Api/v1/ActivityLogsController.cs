using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PracticalWork.Reports.Abstractions.Services.Domain;
using PracticalWork.Reports.Contracts.v1.Abstracts;
using PracticalWork.Reports.Contracts.v1.ActivityLogs.Get;
using PracticalWork.Reports.Controllers.Mappers.v1;

namespace PracticalWork.Reports.Controllers.Api.v1;

/// <summary>
/// Контроллер для работы с отчетами
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/activityLogs")]
public class ActivityLogsController : Controller
{
    private readonly IActivityLogService _activityLogService;

    public ActivityLogsController(IActivityLogService activityLogService)
    {
        _activityLogService = activityLogService;
    }
     
    /// <summary>
    /// Получение логов активности
    /// </summary>
    /// <param name="request">Запрос на получение логов активности, содержащий параметры фильтрации и пагинации</param>
    /// <returns>Страница с логами активности</returns>
    [HttpGet]
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
}