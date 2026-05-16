using PracticalWork.Reports.Contracts.v1.Enums;

namespace PracticalWork.Reports.Contracts.v1.Reports.Generate;

/// <summary>
/// Запрос на генерацию отчета с данными об активности библиотеки
/// </summary>
/// <param name="PeriodFrom">Начало периода отчетности (формат ввода: yyyy-mm-dd)</param>
/// <param name="PeriodTo">Конец периода отчетности (формат ввода: yyyy-mm-dd)</param>
/// <param name="EventType">Тип события</param>
public sealed record GenerateLibraryActivityReportRequest(
    DateOnly PeriodFrom,
    DateOnly PeriodTo,
    ActivityEventType EventType);