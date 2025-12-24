using PracticalWork.Reports.Abstractions.Services.Domain;
using PracticalWork.Reports.Abstractions.Storage;
using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Services;

public class ActivityLogService: IActivityLogService
{
    private readonly IActivityLogRepository _activityLogRepository;

    public ActivityLogService(IActivityLogRepository activityLogRepository)
    {
        _activityLogRepository = activityLogRepository;
    }
    public async Task<PageDto<ActivityLog>> GetPagedActivityLogs(ActivityLogFilterDto filter, PaginationDto pagination)
    {
        var activityLogs = await _activityLogRepository.GetActivityLogs(filter, pagination);

        return new PageDto<ActivityLog>
        {
            PageSize = pagination.PageSize,
            Page = pagination.Page,
            Items = activityLogs
        };
    }
}