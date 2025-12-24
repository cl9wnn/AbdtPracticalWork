using Microsoft.EntityFrameworkCore;
using PracticalWork.Reports.Abstractions.Storage;
using PracticalWork.Reports.Data.PostgreSql.Entities;
using PracticalWork.Reports.Data.PostgreSql.Mappers.v1;
using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Enums;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Data.PostgreSql.Repositories;

public class ActivityLogRepository : IActivityLogRepository
{
    private readonly AppDbContext _appDbContext;

    public ActivityLogRepository(AppDbContext dbContext)
    {
        _appDbContext = dbContext;
    }

    public async Task<Guid> Add(ActivityLog activityLog, Guid? bookId = null, Guid? readerId = null)
    {
        var activityLogEntity = activityLog.ToActivityLogEntity(bookId, readerId);

        _appDbContext.ActivityLogs.Add(activityLogEntity);
        await _appDbContext.SaveChangesAsync();

        return activityLogEntity.Id;
    }

    public async Task<IReadOnlyList<ActivityLog>> GetActivityLogs(ActivityLogFilterDto filter, PaginationDto pagination)
    {
        var query = BuildActivityLogsQuery(filter);

        return await query
            .OrderBy(b => b.EventDate)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(b => b.ToActivityLog())
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IReadOnlyList<ActivityLog>> GetActivityLogsByPeriodAsync(DateOnly from, DateOnly to,
        ActivityEventType eventType)
    {
        var fromDate = from.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var toDate = to.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);

        return await _appDbContext.ActivityLogs
            .Where(x => x.EventDate >= fromDate && x.EventDate <= toDate && x.EventType == eventType)
            .OrderBy(x => x.EventDate)
            .Select(a => a.ToActivityLog())
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>Построение запроса для поиска логов активности по фильтрации</summary>
    private IQueryable<ActivityLogEntity> BuildActivityLogsQuery(ActivityLogFilterDto filter)
    {
        IQueryable<ActivityLogEntity> query = _appDbContext.ActivityLogs;

        if (filter.EventType.HasValue)
        {
            query = query.Where(x => x.EventType == filter.EventType.Value);
        }

        if (filter.EventDate.HasValue)
        {
            var date = filter.EventDate.Value.Date;

            query = query.Where(x =>
                x.EventDate >= date &&
                x.EventDate < date.AddDays(1));
        }

        return query;
    }
}