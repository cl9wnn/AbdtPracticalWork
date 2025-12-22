using PracticalWork.Reports.Enums;

namespace PracticalWork.Reports.Dtos;

public class GenerateReportDto
{
    public DateOnly PeriodFrom { get; set; }
    public DateOnly PeriodTo { get; set; }
    public ActivityEventType EventType { get; set; }
}