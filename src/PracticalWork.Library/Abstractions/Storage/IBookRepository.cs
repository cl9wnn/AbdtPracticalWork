using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;
using PracticalWork.Library.SharedKernel.Abstractions;

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
    /// <returns>Детальная информация о книге</returns>
    Task<BookDetailsDto> GetByTitle(string title);
     
    /// <summary>
    /// Получение списка книг с учетом фильтров и пагинации
    /// </summary>
    /// <param name="filter">Фильтр поиска</param>
    /// <param name="pagination">Параметры пагинации</param>
    /// <returns>Список не архивных книг из библиотеки</returns>
    Task<IReadOnlyList<BookListDto>> GetBooks(BookFilterDto filter, PaginationDto pagination);
    
    /// <summary>
    /// Получение списка не архивных книг из библиотеки с учетом фильтров и пагинации
    /// </summary>
    /// <param name="filter">Фильтр поиска</param>
    /// <param name="pagination">Параметры пагинации</param>
    /// <returns>Список не архивных книг из библиотеки</returns>
    Task<IReadOnlyList<LibraryBookDto>> GetLibraryBooks(BookFilterDto filter, PaginationDto pagination);
}