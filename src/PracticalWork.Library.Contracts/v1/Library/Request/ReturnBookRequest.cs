namespace PracticalWork.Library.Contracts.v1.Library.Request;

/// <summary>
/// Запрос на возврат книги читателем
/// </summary>
/// <param name="BookId">Идентификатор возвращаемой книги</param>
public sealed record ReturnBookRequest(Guid BookId);