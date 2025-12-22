using PracticalWork.Reports.Contracts.v1.Enums;

namespace PracticalWork.Reports.Contracts.v1.ActivityLogs.Get;

public sealed record GetActivityLogsRequest(
    ActivityEventType? EventType, 
    DateTime? EventDate, 
    int Page, 
    int PageSize);