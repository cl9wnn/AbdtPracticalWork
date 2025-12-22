using PracticalWork.Reports.Contracts.v1.Enums;

namespace PracticalWork.Reports.Contracts.v1.Abstracts;

public abstract record AbstractReport(string Name, string FilePath, ReportStatus Status, DateTime GeneratedAt);