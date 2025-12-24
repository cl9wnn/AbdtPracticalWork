using PracticalWork.Reports.Data.PostgreSql.Entities;
using PracticalWork.Reports.Enums;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Data.PostgreSql.Mappers.v1;

public static class ReportsExtensions
{
    public static ReportEntity ToReportEntity(this Report report)
    {
        return new ReportEntity
        {
            CreatedAt = DateTime.UtcNow,
            Name = report.Name,
            FilePath = report.FilePath,
            Status = report.Status,
            GeneratedAt = report.GeneratedAt,
            PeriodFrom = report.PeriodFrom,
            PeriodTo = report.PeriodTo
        };
    }

    public static Report ToReport(this ReportEntity reportEntity)
    {
        return new Report
        {
            Name = reportEntity.Name,
            FilePath = reportEntity.FilePath,
            Status = reportEntity.Status,
            GeneratedAt = reportEntity.GeneratedAt,
            PeriodFrom = reportEntity.PeriodFrom,
            PeriodTo = reportEntity.PeriodTo
        };
    }
    
    public static ActivityLogEntity ToActivityLogEntity(this ActivityLog activityLog, Guid? bookId, Guid? readerId)
    {
        return new ActivityLogEntity
        {
            EventType = activityLog.EventType,
            EventDate = activityLog.EventDate,
            Metadata = activityLog.Metadata,
            CreatedAt = DateTime.UtcNow,
            BookId = bookId,
            ReaderId = readerId
        };
    }

    public static ActivityLog ToActivityLog(this ActivityLogEntity activityLogEntity)
    {
        return new ActivityLog
        {
            EventType = activityLogEntity.EventType,
            EventDate = activityLogEntity.EventDate,
            Metadata = activityLogEntity.Metadata
        };
    }
}