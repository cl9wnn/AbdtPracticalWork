namespace PracticalWork.Library.Contracts.v1.Library.Borrow;

/// <summary>
/// Ответ на запрос о выдаче книги читателю
/// </summary>
/// <param name="Id">Идентификатор выдачи книги</param>
public sealed record BorrowBookResponse(Guid Id);