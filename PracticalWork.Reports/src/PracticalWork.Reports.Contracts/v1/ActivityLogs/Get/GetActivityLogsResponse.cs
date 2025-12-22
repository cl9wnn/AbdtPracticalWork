using System.Text.Json;
using PracticalWork.Reports.Contracts.v1.Enums;

namespace PracticalWork.Reports.Contracts.v1.ActivityLogs.Get;

public record GetActivityLogsResponse(ActivityEventType EventType, DateTime EventDate, JsonDocument Metadata);
