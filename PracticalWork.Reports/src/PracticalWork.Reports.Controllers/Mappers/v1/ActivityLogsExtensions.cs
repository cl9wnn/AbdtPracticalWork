using PracticalWork.Reports.Contracts.v1.ActivityLogs.Get;
using PracticalWork.Reports.Contracts.v1.Enums;
using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Controllers.Mappers.v1;

public static class ActivityLogsExtensions
{
    public static ActivityLogFilterDto ToActivityLogFilterDto(this GetActivityLogsRequest request) =>
        new()
        {
            EventType = (Enums.ActivityEventType?)request.EventType,
            EventDate = request.EventDate
        };

    public static PaginationDto ToActivityLogPaginationDto(this GetActivityLogsRequest request) =>
        new()
        {
            Page = request.Page > 0 ? request.Page : 1,
            PageSize = request.PageSize is > 0 and <= 100 ? request.PageSize : 20
        };

    public static GetActivityLogsResponse ToActivityLogsResponse(this ActivityLog activityLog) =>
        new(
            EventType: (ActivityEventType)activityLog.EventType,
            EventDate: activityLog.EventDate,
            Metadata: activityLog.Metadata
        );

    public static IReadOnlyList<GetActivityLogsResponse> ToActivityLogsResponseList(
        this IEnumerable<ActivityLog> activityLogs) =>
        activityLogs.Select(b => b.ToActivityLogsResponse()).ToList();
}