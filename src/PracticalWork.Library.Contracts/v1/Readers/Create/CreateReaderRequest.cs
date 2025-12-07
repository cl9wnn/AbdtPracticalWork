namespace PracticalWork.Library.Contracts.v1.Readers.Create;

/// <summary>
/// Запрос на создание карточки читателя
/// </summary>
/// <param name="FullName">ФИО читателя</param>
/// <param name="PhoneNumber">Номер телефона читателя</param>
/// <param name="ExpiryDate">Дата окончания действия карточки</param>
public sealed record CreateReaderRequest(string FullName, string PhoneNumber, DateOnly ExpiryDate);