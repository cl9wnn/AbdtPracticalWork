namespace PracticalWork.Library.Contracts.v1.Readers.Create;

/// <summary>
/// Ответ на создание карточки читателя
/// </summary>
/// <param name="Id">Идентификатор карточки читателя</param>
public sealed record CreateReaderResponse(Guid Id);
