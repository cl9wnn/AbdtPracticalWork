using PracticalWork.Reports.Abstractions.Services.Domain;
using PracticalWork.Reports.Abstractions.Storage;
using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Enums;
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

    /// <inheritdoc cref="IActivityLogService.GetWeeklyStatistics"/>
    public async Task<WeeklyStatisticsDto> GetWeeklyStatistics(DateOnly from, DateOnly to, 
        CancellationToken cancellationToken)
    {
        var eventTypesCountDict = 
            await _activityLogRepository.GetActivityEventTypeCountsByPeriod(from, to, cancellationToken);

        return new WeeklyStatisticsDto
        {
            NewBooksCount = eventTypesCountDict.GetValueOrDefault(ActivityEventType.BookCreated),
            NewReadersCount = eventTypesCountDict.GetValueOrDefault(ActivityEventType.ReaderCreated),
            BorrowedBooksCount = eventTypesCountDict.GetValueOrDefault(ActivityEventType.BookBorrowed),
            ReturnedBooksCount = eventTypesCountDict.GetValueOrDefault(ActivityEventType.BookReturned),
            OverdueBooksCount = 0
        };
    }
}