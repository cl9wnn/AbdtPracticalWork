using Microsoft.AspNetCore.Http;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;
using PracticalWork.Library.SharedKernel.Abstractions;

namespace PracticalWork.Library.Abstractions.Services;

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
    Task<ArchiveBookDto> ArchiveBook(Guid bookId);

    /// <summary>
    /// Получение списка книг
    /// </summary>
    /// <param name="filter">Фильтр поиска</param>
    /// <param name="page">Номер страницы</param>
    /// <param name="pageSize">Размер страницы</param>
    /// <returns>Список найденных книг</returns>
    Task<IReadOnlyList<Book>> GetBooks(BookFilterDto filter, int page, int pageSize);

    /// <summary>
    /// Добавление деталей книги
    /// </summary>
    /// <param name="bookId">Идентификатор книги</param>
    /// <param name="coverImage">Обложка книги</param>
    /// <param name="description">Описание книги</param>
    /// <returns>Информация об измененной книге</returns>
    Task<BookDetailsDto> AddBookDetails(Guid bookId, IFormFile coverImage, string description);
}