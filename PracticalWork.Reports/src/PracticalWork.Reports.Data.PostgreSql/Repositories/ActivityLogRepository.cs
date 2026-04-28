using Microsoft.EntityFrameworkCore;
using PracticalWork.Reports.Abstractions.Storage;
using PracticalWork.Reports.Data.PostgreSql.Entities;
using PracticalWork.Reports.Data.PostgreSql.Mappers.v1;
using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Enums;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Data.PostgreSql.Repositories;

/// <summary>
/// Репозиторий для работы с логами активности
/// </summary>
public class ActivityLogRepository : IActivityLogRepository
{
    private readonly AppDbContext _appDbContext;

    public ActivityLogRepository(AppDbContext dbContext)
    {
        _appDbContext = dbContext;
    }
 
    /// <inheritdoc cref="IActivityLogRepository.Add"/>
    public async Task<Guid> Add(ActivityLog activityLog, Guid? bookId = null, Guid? readerId = null)
    {
        var activityLogEntity = activityLog.ToActivityLogEntity(bookId, readerId);

        _appDbContext.ActivityLogs.Add(activityLogEntity);
        await _appDbContext.SaveChangesAsync();

        return activityLogEntity.Id;
    }

    /// <inheritdoc cref="IActivityLogRepository.GetActivityLogs"/>
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

    /// <inheritdoc cref="IActivityLogRepository.GetActivityLogsByPeriod"/>
    public async Task<IReadOnlyList<ActivityLog>> GetActivityLogsByPeriod(DateOnly from, DateOnly to,
        ActivityEventType eventType)
    {
        return await _appDbContext.ActivityLogs
            .Where(x => x.EventDate >= from && x.EventDate <= to && x.EventType == eventType)
            .OrderBy(x => x.EventDate)
            .Select(a => a.ToActivityLog())
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc cref="IActivityLogRepository.GetActivityEventTypeCountsByPeriod"/>
    public async Task<IReadOnlyDictionary<ActivityEventType, int>> GetActivityEventTypeCountsByPeriod (
        DateOnly from,
        DateOnly to)
    {
        return await _appDbContext.ActivityLogs
            .Where(x => x.EventDate >= from && x.EventDate <= to)
            .GroupBy(x => x.EventType)
            .Select(g => new
            {
                EventType = g.Key,
                Count = g.Count()
            })
            .ToDictionaryAsync(x => x.EventType, x => x.Count);
    }
    
    /// <summary>
    /// Построение запроса для поиска логов активности по фильтрации
    /// </summary>
    private IQueryable<ActivityLogEntity> BuildActivityLogsQuery(ActivityLogFilterDto filter)
    {
        IQueryable<ActivityLogEntity> query = _appDbContext.ActivityLogs;

        if (filter.EventType.HasValue)
        {
            query = query.Where(x => x.EventType == filter.EventType.Value);
        }

        if (filter.EventDate.HasValue)
        {
            var date = filter.EventDate.Value;

            query = query.Where(x =>
                x.EventDate >= date &&
                x.EventDate < date.AddDays(1));
        }

        return query;
    }
}