using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;
using PracticalWork.Shared.Abstractions.Interfaces;

namespace PracticalWork.Library.Abstractions.Storage;

/// <summary>
/// Контракт репозитория для работы с книгами
/// </summary>
public interface IBookRepository: IEntityRepository<Guid, Book>
{
    /// <summary>
    /// Получение детальной информации о книги по ее названию
    /// </summary>
    /// <param name="title">Название книги</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Детальная информация о книге</returns>
    Task<BookDetailsDto> GetByTitle(string title, CancellationToken cancellationToken = default);
     
    /// <summary>
    /// Получение списка книг с учетом фильтров и пагинации
    /// </summary>
    /// <param name="filter">Фильтр поиска</param>
    /// <param name="pagination">Параметры пагинации</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список не архивных книг из библиотеки</returns>
    Task<IReadOnlyList<BookListDto>> GetBooks(BookFilterDto filter, PaginationDto pagination,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получение количества новых книг за определенный период 
    /// </summary>
    /// <param name="from">Дата начала периода</param>
    /// <param name="to">Дата конца периода</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Количество новых книг</returns>
    Task<int> GetNewBooksCount(DateTime from, DateTime to,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получение списка не архивных книг из библиотеки с учетом фильтров и пагинации
    /// </summary>
    /// <param name="filter">Фильтр поиска</param>
    /// <param name="pagination">Параметры пагинации</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список не архивных книг из библиотеки</returns>
    Task<IReadOnlyList<LibraryBookDto>> GetLibraryBooks(BookFilterDto filter, PaginationDto pagination,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получение списка старых книг, подлежащих архивации
    /// </summary>
    /// <param name="thresholdDate">Пороговая дата</param>
    /// <param name="limit">Лимит на количество</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns></returns>
    Task<IReadOnlyList<(Guid, Book)>> GetBooksForArchiving(DateTime thresholdDate, int limit, 
        CancellationToken cancellationToken = default);
}