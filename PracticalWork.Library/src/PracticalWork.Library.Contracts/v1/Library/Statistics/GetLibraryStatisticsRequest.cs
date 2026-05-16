namespace PracticalWork.Library.Contracts.v1.Library.Statistics;

/// <summary>
/// Запрос на получение статистики библиотеки
/// </summary>
/// <param name="PeriodFrom">Начало периода статистики (формат ввода: yyyy-mm-dd)</param>
/// <param name="PeriodTo">Конец периода статистики (формат ввода: yyyy-mm-dd)</param>
public sealed record GetLibraryStatisticsRequest(
    DateOnly PeriodFrom,
    DateOnly PeriodTo);