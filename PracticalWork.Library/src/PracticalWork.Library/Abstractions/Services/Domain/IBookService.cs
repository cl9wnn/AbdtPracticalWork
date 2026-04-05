using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;
using PracticalWork.Library.SharedKernel.Abstractions;

namespace PracticalWork.Library.Abstractions.Services.Domain;

/// <summary>
/// Контракт сервиса по работе с книгами
/// </summary>
public interface IBookService : IEntityService<Book>
{
    /// <summary>
    /// Создание книги
    /// </summary>
    /// <param name="book">Книга</param>
    /// <returns>Идентификатор созданной книги</returns>
    Task<Guid> CreateBook(Book book);

    /// <summary>
    /// Редактирование книги
    /// </summary>
    /// <param name="bookId">Идентификатор книги</param>
    /// <param name="book">Книга</param>
    Task UpdateBook(Guid bookId, Book book);

    /// <summary>
    /// Перевод книги в архив 
    /// </summary>
    /// <param name="bookId">Идентификатор книги</param>
    /// <returns>Информация об архивированной книге</returns>
    Task<ArchivedBookDto> ArchiveBook(Guid bookId);
    
    /// <summary>
    /// Архивирование старых книг
    /// </summary>
    /// <param name="yearsWithoutBorrow">
    /// Количество лет, в течение которых книга не выдавалась, чтобы считаться
    /// кандидатом на архивацию</param>
    /// <param name="maxBooksPerRun">
    /// Максимальное количество книг для обработки за один запуск
    /// </param>
    /// <returns>Отчет об итогах архивации книг</returns>
    Task<ArchiveReportDto> ArchiveOldBooks(int yearsWithoutBorrow, int maxBooksPerRun);

    /// <summary>
    /// Получение списка книг с учетом фильтров и пагинации
    /// </summary>
    /// <param name="filter">Фильтр поиска</param>
    /// <param name="pagination">Параметры пагинации</param>
    /// <returns>Страница со списком книг</returns>
    Task<PageDto<BookListDto>> GetBooksPage(BookFilterDto filter, PaginationDto pagination);

    /// <summary>
    /// Добавление деталей книги
    /// </summary>
    /// <param name="bookId">Идентификатор книги</param>
    /// <param name="description">Описание книги</param>
    /// <param name="coverImageStream">Поток данных с обложкой книги</param>
    /// <param name="contentType">Тип изображения</param>
    Task AddBookDetails(Guid bookId, string description, Stream coverImageStream, string contentType);
}