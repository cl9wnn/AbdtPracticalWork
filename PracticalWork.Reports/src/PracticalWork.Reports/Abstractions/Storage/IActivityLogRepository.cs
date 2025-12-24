using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Enums;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Abstractions.Storage;

public interface IActivityLogRepository
{
    Task<Guid> Add(ActivityLog activityLog, Guid? bookId = null, Guid? readerId = null);
    Task<IReadOnlyList<ActivityLog>> GetActivityLogs(ActivityLogFilterDto filter, PaginationDto pagination);
    Task<IReadOnlyList<ActivityLog>> GetActivityLogsByPeriodAsync(DateOnly from, DateOnly to,
        ActivityEventType eventType);
}