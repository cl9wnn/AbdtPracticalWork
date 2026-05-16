using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;
using PracticalWork.Shared.Abstractions.Interfaces;

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
    
    /// <summary>
    /// Получение количества новых карточек читателей за определенный период
    /// </summary>
    /// <param name="from">Дата начала периода</param>
    /// <param name="to">Дата конца периода</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Количество новых карточек</returns>
    Task<int> GetNewReadersCount(DateTime from, DateTime to,
        CancellationToken cancellationToken = default);
}