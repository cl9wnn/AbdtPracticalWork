using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;
using PracticalWork.Library.SharedKernel.Abstractions;

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
    /// <returns>Идентификатор записи о выдаче</returns>
    Task<Guid> Create(Guid bookId, Guid readerId, Borrow bookBorrow);
    
    /// <summary>
    /// Получение действующей записи о выдаче книги по ее идентификатору
    /// </summary>
    /// <param name="bookId">Идентификатор взятой книги</param>
    /// <returns>Действующая запись о выдаче книги</returns>
    Task<Borrow> GetActiveBorrowByBookId(Guid bookId);
    
    /// <summary>
    /// Обновление записи о выдаче книги читателю
    /// </summary>
    /// <param name="bookId">Идентификатор взятой книги</param>
    /// <param name="bookBorrow">Запись о выдаче книги</param>
    Task Update(Guid bookId, Borrow bookBorrow);

    /// <summary>
    /// Получение информации о читателе, взявшем книгу
    /// </summary>
    /// <param name="borrowedBookId"></param>
    /// <returns>Краткая информация о читателе</returns>
    Task<ReaderInfoDto> GetReaderInfoByBorrowedBookId(Guid borrowedBookId);

}