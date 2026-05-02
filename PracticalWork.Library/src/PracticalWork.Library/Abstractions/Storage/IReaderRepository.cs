using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;
using PracticalWork.Library.SharedKernel.Abstractions;

namespace PracticalWork.Library.Abstractions.Storage;

/// <summary>
/// Контракт репозитория для работы с карточками читателей
/// </summary>
public interface IReaderRepository: IEntityRepository<Guid, Reader>
{
    /// <summary>
    /// Проверка существования карточки читателя по номеру телефона
    /// </summary>
    /// <param name="phoneNumber">Идентификатор сущности</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task<bool> Exists(string phoneNumber, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получение списка всех взятых читателем книг 
    /// </summary>
    /// <param name="readerId">Идентификатор карточки читателя</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список взятых читателем книг</returns>
    Task<IReadOnlyList<BorrowedBookDto>> GetBorrowedBooks(Guid readerId, CancellationToken cancellationToken = default);
}