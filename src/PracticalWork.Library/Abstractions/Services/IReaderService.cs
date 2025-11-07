using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;
using PracticalWork.Library.SharedKernel.Abstractions;

namespace PracticalWork.Library.Abstractions.Services;

/// <summary>
/// Сервис по работе с карточкой читателя
/// </summary>
public interface IReaderService: IEntityService<Reader>
{
    /// <summary>
    /// Создание карточки читателя
    /// </summary>
    /// <param name="reader">Карточка читателя</param>
    /// <returns>Идентификатор созданной карточки читателя</returns>
    Task<Guid> CreateReader(Reader reader);
    
    /// <summary>
    /// Продление срока действия карточки читателя
    /// </summary>
    /// <param name="readerId">Идентификатор карточки читателя</param>
    /// <param name="newExpiryDate">Новая дата окончания действия карточки</param>
    Task ExtendReader(Guid readerId,  DateOnly newExpiryDate);
    
    /// <summary>
    /// Закрытие карточки читателя
    /// </summary>
    /// <param name="readerId">Идентификатор карточки читателя</param>
    Task CloseReader(Guid readerId);
    
    /// <summary>
    /// Получение взятых книг
    /// </summary>
    /// <param name="readerId">Идентификатор карточки читателя</param>
    /// <returns>Список взятых книг</returns>
    Task<IReadOnlyList<BorrowedBookDto>> GetBorrowedBooks(Guid readerId);
}