using PracticalWork.Reports.Contracts.v1.Enums;

namespace PracticalWork.Reports.Contracts.v1.Reports.Generate;

/// <summary>
/// Запрос на генерацию отчета
/// </summary>
/// <param name="PeriodFrom">Начало периода отчетности</param>
/// <param name="PeriodTo">Конец периода отчетности</param>
/// <param name="EventType">Тип события</param>
public sealed record GenerateReportRequest(DateOnly PeriodFrom, DateOnly PeriodTo, ActivityEventType EventType);