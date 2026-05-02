using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;
using PracticalWork.Library.SharedKernel.Abstractions;

namespace PracticalWork.Library.Abstractions.Services.Domain;

/// <summary>
/// Контракт сервиса по работе с библиотекой
/// </summary>
public interface ILibraryService
{
    /// <summary>
    /// Получение списка книг библиотеки
    /// </summary>
    /// <param name="filter">Фильтр поиска</param>
    /// <param name="pagination">Параметры пагинации</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Страница со списком найденных книг в библиотеке</returns>
    Task<PageDto<LibraryBookDto>> GetLibraryBooksPage(BookFilterDto filter, PaginationDto pagination, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Выдача книги
    /// </summary>
    /// <param name="bookId">Идентификатор выдаваемой книги</param>
    /// <param name="readerId">Идентификатор карточки читателя</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Идентификатор выдачи книги читателю</returns>
    Task<Guid> BorrowBook(Guid bookId, Guid readerId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Возврат книги
    /// </summary>
    /// <param name="bookId">Идентификатор возвращаемой книги</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task ReturnBook(Guid bookId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получение деталей книги по ее идентификатору
    /// </summary>
    /// <param name="bookId"></param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Детальная информация о книге</returns>
    Task<BookDetailsDto> GetBookDetailsById(Guid bookId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получение деталей книги по ее названию
    /// </summary>
    /// <param name="title">Название книги</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Детальная информация о книге</returns>
    Task<BookDetailsDto> GetBookDetailsByTitle(string title, CancellationToken cancellationToken = default);
}