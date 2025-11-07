namespace PracticalWork.Library.Contracts.v1.Readers.Request;

/// <summary>
/// Запрос на продление срока карточки читателя
/// </summary>
/// <param name="NewExpiryDate">Новая дата окончания действия</param>
public sealed record ExtendReaderRequest(DateOnly NewExpiryDate);
