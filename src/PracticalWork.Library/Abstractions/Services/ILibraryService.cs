using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;
using PracticalWork.Library.SharedKernel.Abstractions;

namespace PracticalWork.Library.Abstractions.Services;

public interface ILibraryService: IEntityService<Borrow>
{
    /// <summary>
    /// Получение списка книг библиотеки
    /// </summary>
    /// <param name="filter">Фильтр поиска</param>
    /// <param name="page">Номер страницы</param>
    /// <param name="pageSize">Размер страницы</param>
    /// <returns>Список найденных книг в библиотеке</returns>
    Task<IReadOnlyList<LibraryBookDto>> GetLibraryBooks(BookFilterDto filter, int page, int pageSize);
    
    /// <summary>
    /// Выдача книги
    /// </summary>
    /// <param name="bookId">Идентификатор выдаваемой книги</param>
    /// <param name="readerId">Идентификатор карточки читателя</param>
    /// <returns>Идентификатор выдачи книги читателю</returns>
    Task<Guid> BorrowBook(Guid bookId, Guid readerId);
    
    /// <summary>
    /// Возврат книги
    /// </summary>
    /// <param name="bookId">Идентификатор возвращаемой книги</param>
    Task ReturnBook(Guid bookId);
    
    /// <summary>
    /// Получение деталей книги по ее идентификатору
    /// </summary>
    /// <param name="bookId"></param>
    /// <returns>Детальная информация о книге</returns>
    Task<BookDetailsDto> GetBookDetailsById(Guid bookId);
    
    /// <summary>
    /// Получение деталей книги по ее названию
    /// </summary>
    /// <param name="title">Название книги</param>
    /// <returns>Детальная информация о книге</returns>
    Task<BookDetailsDto> GetBookDetailsByTitle(string title);
}