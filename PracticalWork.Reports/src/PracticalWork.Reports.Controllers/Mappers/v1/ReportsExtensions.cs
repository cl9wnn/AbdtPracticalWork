using PracticalWork.Reports.Contracts.v1.ActivityLogs.Get;
using PracticalWork.Reports.Contracts.v1.Enums;
using PracticalWork.Reports.Contracts.v1.Reports.Generate;
using PracticalWork.Reports.Contracts.v1.Reports.Get;
using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Controllers.Mappers.v1;

public static class ReportsExtensions
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

    public static GetReportResponse ToGetReportResponse(this Report report) =>
        new(
            Name: report.Name, 
            FilePath: report.FilePath, 
            Status: (ReportStatus)report.Status, 
            GeneratedAt: report.GeneratedAt
        );
    
    public static IReadOnlyList<GetReportResponse> ToGetReportResponseList(
        this IEnumerable<Report> reports) =>
        reports.Select(b => b.ToGetReportResponse()).ToList();
    
    public static GenerateReportDto ToGenerateReportDto(this GenerateReportRequest request) =>
        new ()
        {
            PeriodFrom = request.PeriodFrom,
            PeriodTo =request.PeriodTo,
            EventType = (Enums.ActivityEventType)request.EventType
        };

    public static GenerateReportResponse ToGenerateReportResponse(this Report report) =>
        new(
             Name: report.Name, 
             FilePath: report.FilePath, 
             Status: (ReportStatus)report.Status, 
             GeneratedAt: report.GeneratedAt
        );

}