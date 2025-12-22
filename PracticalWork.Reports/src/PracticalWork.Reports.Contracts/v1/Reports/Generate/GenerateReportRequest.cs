using PracticalWork.Reports.Contracts.v1.Enums;

namespace PracticalWork.Reports.Contracts.v1.Reports.Generate;

public sealed record GenerateReportRequest(DateOnly PeriodFrom, DateOnly PeriodTo, ActivityEventType EventType);