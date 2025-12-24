namespace PracticalWork.Reports.Dtos;

public sealed class ActivityLogReportRow
{
    public DateTime EventDate { get; init; }
    public string EventType { get; init; }
    public string Metadata { get; init; }
}