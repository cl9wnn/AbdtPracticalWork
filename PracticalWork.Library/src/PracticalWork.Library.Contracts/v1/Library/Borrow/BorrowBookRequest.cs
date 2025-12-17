namespace PracticalWork.Library.Contracts.v1.Library.Borrow;

/// <summary>
/// Запрос на выдачу книги на руки читателю
/// </summary>
/// <param name="BookId">Идентификатор книги</param>
/// <param name="ReaderId">Идентификатор карточки читателя</param>
public sealed record BorrowBookRequest(Guid BookId, Guid ReaderId);