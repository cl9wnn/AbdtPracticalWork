using PracticalWork.Reports.Abstractions.Services.Domain;
using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Services;

public class ActivityLogService: IActivityLogService
{
    public Task<PageDto<ActivityLog>> GetPagedActivityLogs(ActivityLogFilterDto filter, PaginationDto pagination)
    {
        throw new NotImplementedException();
    }
}