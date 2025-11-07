namespace PracticalWork.Library.Contracts.v1.Readers.Response;

/// <summary>
/// Ответ на запрос о получении взятой книги
/// </summary>
/// <param name="BookId">Идентификатор книги</param>
/// <param name="BorrowDate">Дата выдачи книги</param>
/// <param name="DueDate">Срок возврата книги</param>
public sealed record GetBorrowedBookResponse(Guid BookId, DateOnly BorrowDate, DateOnly DueDate);