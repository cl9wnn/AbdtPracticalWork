namespace PracticalWork.Library.Contracts.v1.Library.Statistics;

/// <summary>
/// Ответ на получение статистики активности библиотеки
/// </summary>
/// <param name="NewBooksCount">Количество новых книг</param>
/// <param name="NewReadersCount">Количество новых читателей</param>
/// <param name="BorrowedBooksCount">Количество выданных книг</param>
/// <param name="ReturnedBooksCount">Количество возвращенных книг</param>
/// <param name="OverdueBooksCount">Количество просроченных выдач</param>
public sealed record GetLibraryStatisticsResponse(
    int NewBooksCount,
    int NewReadersCount,
    int BorrowedBooksCount,
    int ReturnedBooksCount,
    int OverdueBooksCount
);