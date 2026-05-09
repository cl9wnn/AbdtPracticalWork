using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;

namespace PracticalWork.Library.Abstractions.Storage;

/// <summary>
/// Контракт репозитория для работы с записями о выдаче книг читателям
/// </summary>
public interface IBookBorrowRepository
{
    /// <summary>
    /// Создание записи о выдаче книги читателю
    /// </summary>
    /// <param name="bookId">Идентификатор взятой книги</param>
    /// <param name="readerId">Идентификатор карточки читателя</param>
    /// <param name="bookBorrow">Запись о выдаче книги</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Идентификатор записи о выдаче</returns>
    Task<Guid> Create(Guid bookId, Guid readerId, Borrow bookBorrow, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получение действующей записи о выдаче книги по ее идентификатору
    /// </summary>
    /// <param name="bookId">Идентификатор взятой книги</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Действующая запись о выдаче книги</returns>
    Task<Borrow> GetActiveBorrowByBookId(Guid bookId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Обновление записи о выдаче книги читателю
    /// </summary>
    /// <param name="bookId">Идентификатор взятой книги</param>
    /// <param name="bookBorrow">Запись о выдаче книги</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task Update(Guid bookId, Borrow bookBorrow, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение информации о читателе, взявшем книгу
    /// </summary>
    /// <param name="borrowedBookId"></param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Краткая информация о читателе</returns>
    Task<ReaderInfoDto> GetReaderInfoByBorrowedBookId(Guid borrowedBookId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение списка активных выдач книг, срок возврата которых наступает через n дней 
    /// </summary>
    /// <param name="days">Количество дней до возврата книги</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns></returns>
    Task<IReadOnlyList<BorrowedBookDto>> GetBorrowsDueInDays(int days, CancellationToken cancellationToken = default);
}