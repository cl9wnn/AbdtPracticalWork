using PracticalWork.Reports.Contracts.v1.Enums;
using PracticalWork.Reports.Contracts.v1.Reports.Generate;
using PracticalWork.Reports.Contracts.v1.Reports.Get;
using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Models;
using PracticalWork.Shared.Contracts.Http.Reports.WeeklyReport;

namespace PracticalWork.Reports.Controllers.Mappers.v1;

public static class ReportsExtensions
{
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

    public static GenerateLibraryActivityReportDto ToGenerateLibraryActivityReportDto(
        this GenerateLibraryActivityReportRequest request) =>
        new()
        {
            PeriodFrom = request.PeriodFrom,
            PeriodTo = request.PeriodTo,
            EventType = (Enums.ActivityEventType)request.EventType
        };

    public static GenerateWeeklyReportDto ToGenerateWeeklyReportDto(this GenerateWeeklyReportRequest request) =>
        new()
        {
            PeriodFrom = request.PeriodFrom,
            PeriodTo = request.PeriodTo,
            WeeklyStatistics = new WeeklyStatisticsDto
            {
                NewBooksCount = request.NewBooksCount,
                NewReadersCount = request.NewReadersCount,
                BorrowedBooksCount = request.BorrowedBooksCount,
                ReturnedBooksCount = request.ReturnedBooksCount,
                OverdueBooksCount = request.OverdueBooksCount
            }
        };

    public static GenerateLibraryActivityReportResponse ToGenerateLibraryActivityReportResponse(this Report report) =>
        new(
            Name: report.Name,
            FilePath: report.FilePath,
            Status: (ReportStatus)report.Status,
            GeneratedAt: report.GeneratedAt
        );

    public static GenerateWeeklyReportResponse ToGenerateWeeklyReportResponse(this WeeklyReportDto report) =>
        new(
            Name: report.Name,
            DownloadUrl: report.DownloadUrl,
            GeneratedAt: report.GeneratedAt
        );
}