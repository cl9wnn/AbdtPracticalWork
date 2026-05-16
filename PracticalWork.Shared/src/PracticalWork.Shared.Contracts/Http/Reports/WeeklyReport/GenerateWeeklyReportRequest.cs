namespace PracticalWork.Shared.Contracts.Http.Reports.WeeklyReport;

/// <summary>
/// Запрос на генерацию отчета с еженедельной статистикой библиотеки
/// </summary>
/// <param name="PeriodFrom">Начало периода отчетности (формат ввода: yyyy-mm-dd)</param>
/// <param name="PeriodTo">Конец периода отчетности (формат ввода: yyyy-mm-dd)</param>
/// <param name="NewBooksCount">Количество новых книг за период</param>
/// <param name="NewReadersCount">Количество новых читателей за период</param>
/// <param name="BorrowedBooksCount">Количество выданных книг за период</param>
/// <param name="ReturnedBooksCount">Количество возвращенных книг за период</param>
/// <param name="OverdueBooksCount">Количество просроченных выдач на конец периода</param>
public sealed record GenerateWeeklyReportRequest(
    DateOnly PeriodFrom,
    DateOnly PeriodTo,
    int NewBooksCount,
    int NewReadersCount,
    int BorrowedBooksCount,
    int ReturnedBooksCount,
    int OverdueBooksCount);