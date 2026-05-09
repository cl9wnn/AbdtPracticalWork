using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;
using PracticalWork.Shared.Abstractions.Interfaces;

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
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Идентификатор созданной книги</returns>
    Task<Guid> CreateBook(Book book, CancellationToken cancellationToken = default);

    /// <summary>
    /// Редактирование книги
    /// </summary>
    /// <param name="bookId">Идентификатор книги</param>
    /// <param name="book">Книга</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task UpdateBook(Guid bookId, Book book, CancellationToken cancellationToken = default);

    /// <summary>
    /// Перевод книги в архив 
    /// </summary>
    /// <param name="bookId">Идентификатор книги</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Информация об архивированной книге</returns>
    Task<ArchivedBookDto> ArchiveBook(Guid bookId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Архивирование старых книг
    /// </summary>
    /// <param name="yearsWithoutBorrow">
    /// Количество лет, в течение которых книга не выдавалась, чтобы считаться
    /// кандидатом на архивацию</param>
    /// <param name="maxBooksPerRun">
    /// Максимальное количество книг для обработки за один запуск
    /// </param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Отчет об итогах архивации книг</returns>
    Task<ArchiveReportDto> ArchiveOldBooks(int yearsWithoutBorrow, int maxBooksPerRun, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение списка книг с учетом фильтров и пагинации
    /// </summary>
    /// <param name="filter">Фильтр поиска</param>
    /// <param name="pagination">Параметры пагинации</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Страница со списком книг</returns>
    Task<PageDto<BookListDto>> GetBooksPage(BookFilterDto filter, PaginationDto pagination, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавление деталей книги
    /// </summary>
    /// <param name="bookId">Идентификатор книги</param>
    /// <param name="description">Описание книги</param>
    /// <param name="coverImageStream">Поток данных с обложкой книги</param>
    /// <param name="contentType">Тип изображения</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task AddBookDetails(Guid bookId, string description, Stream coverImageStream, string contentType, 
        CancellationToken cancellationToken = default);
}