using PracticalWork.Reports.Contracts.v1.Abstracts;
using PracticalWork.Reports.Contracts.v1.Enums;

namespace PracticalWork.Reports.Contracts.v1.Reports.Generate;

/// <summary>
/// Ответ на запрос о генерации отчета
/// </summary>
/// <param name="Name">Название отчета</param>
/// <param name="FilePath">Путь к отчету в MinIO</param>
/// <param name="Status">Статус отчета</param>
/// <param name="GeneratedAt">Дата и время генерации</param>
public sealed record GenerateReportResponse(string Name, string FilePath, ReportStatus Status, DateTime GeneratedAt)
    : AbstractReport(Name, FilePath, Status, GeneratedAt);