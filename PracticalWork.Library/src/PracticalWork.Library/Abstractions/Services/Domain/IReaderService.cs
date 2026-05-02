using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;
using PracticalWork.Library.SharedKernel.Abstractions;

namespace PracticalWork.Library.Abstractions.Services.Domain;

/// <summary>
/// Контракт сервиса по работе с карточкой читателя
/// </summary>
public interface IReaderService: IEntityService<Reader>
{
    /// <summary>
    /// Создание карточки читателя
    /// </summary>
    /// <param name="reader">Карточка читателя</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Идентификатор созданной карточки читателя</returns>
    Task<Guid> CreateReader(Reader reader, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Продление срока действия карточки читателя
    /// </summary>
    /// <param name="readerId">Идентификатор карточки читателя</param>
    /// <param name="newExpiryDate">Новая дата окончания действия карточки</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task ExtendReader(Guid readerId,  DateOnly newExpiryDate, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Закрытие карточки читателя
    /// </summary>
    /// <param name="readerId">Идентификатор карточки читателя</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task CloseReader(Guid readerId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получение взятых книг
    /// </summary>
    /// <param name="readerId">Идентификатор карточки читателя</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список взятых книг</returns>
    Task<IReadOnlyList<BorrowedBookDto>> GetBorrowedBooks(Guid readerId, CancellationToken cancellationToken = default);
}