using PracticalWork.Shared.Contracts.Http.Reports.WeeklyReport;

namespace PracticalWork.Library.Abstractions.Services.Infrastructure;

/// <summary>
/// Контракт клиента для взаимодействия с API сервиса отчетов
/// </summary>
public interface IReportsApiClient
{
    /// <summary>
    /// Генерирует еженедельный отчет по статистике библиотеки
    /// </summary>
    /// <param name="request">Запрос на генерацию отчета</param>
    /// <param name="ct">Токен отмены операции</param>
    /// <returns>Ответ с информацией о генерации</returns>
    Task<GenerateWeeklyReportResponse> GenerateWeeklyReport(
        GenerateWeeklyReportRequest request,
        CancellationToken ct = default);
}