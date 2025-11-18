using Microsoft.AspNetCore.Http;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;
using PracticalWork.Library.SharedKernel.Abstractions;

namespace PracticalWork.Library.Abstractions.Services.Domain;

/// <summary>
/// Сервис по работе с книгами
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
    /// <returns>Информация об измененной книге</returns>
    Task<BookDetailsDto> AddBookDetails(Guid bookId, string description, Stream coverImageStream, string contentType);
}