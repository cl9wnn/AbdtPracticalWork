namespace PracticalWork.Shared.Contracts.Http.Reports.WeeklyReport;

/// <summary>
/// Ответ на запрос о генерации отчета с еженедельной статистикой библиотеки
/// </summary>
/// <param name="Name">Название отчета</param>
/// <param name="DownloadUrl">URL для скачивания</param>
/// <param name="GeneratedAt">Дата и время генерации</param>
public sealed record GenerateWeeklyReportResponse(
    string Name,
    string DownloadUrl,
    DateTime GeneratedAt);
