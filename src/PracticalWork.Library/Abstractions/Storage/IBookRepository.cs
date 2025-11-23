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
    /// Получение списка книг с учетом фильтров и пагинации
    /// </summary>
    /// <param name="filter">Фильтр поиска</param>
    /// <param name="pagination">Параметры пагинации</param>
    Task<IReadOnlyList<BookListDto>> GetBooksPage(BookFilterDto filter, PaginationDto pagination);
}