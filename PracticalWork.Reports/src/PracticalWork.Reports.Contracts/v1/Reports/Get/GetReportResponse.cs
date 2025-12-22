using PracticalWork.Reports.Contracts.v1.Abstracts;
using PracticalWork.Reports.Contracts.v1.Enums;

namespace PracticalWork.Reports.Contracts.v1.Reports.Get;

public sealed record GetReportResponse(string Name, string FilePath, ReportStatus Status, DateTime GeneratedAt)
    : AbstractReport(Name, FilePath, Status, GeneratedAt);