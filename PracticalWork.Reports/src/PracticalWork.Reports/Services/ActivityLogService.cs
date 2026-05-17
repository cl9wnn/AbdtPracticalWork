using PracticalWork.Reports.Abstractions.Services.Domain;
using PracticalWork.Reports.Abstractions.Storage;
using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Services;

/// <summary>
/// Сервис для работы с логами активностей
/// </summary>
public class ActivityLogService: IActivityLogService
{
    private readonly IActivityLogRepository _activityLogRepository;

    public ActivityLogService(IActivityLogRepository activityLogRepository)
    {
        _activityLogRepository = activityLogRepository;
    }
    
    /// <inheritdoc cref="IActivityLogService.GetPagedActivityLogs"/>
    public async Task<PageDto<ActivityLog>> GetPagedActivityLogs(ActivityLogFilterDto filter, PaginationDto pagination,
        CancellationToken cancellationToken)
    {
        var activityLogs = await _activityLogRepository.GetActivityLogs(filter, pagination,
            cancellationToken);

        return new PageDto<ActivityLog>
        {
            PageSize = pagination.PageSize,
            Page = pagination.Page,
            Items = activityLogs
        };
    }
}