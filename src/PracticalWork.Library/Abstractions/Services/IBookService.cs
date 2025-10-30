using Microsoft.AspNetCore.Http;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;
using PracticalWork.Library.SharedKernel.Abstractions;

namespace PracticalWork.Library.Abstractions.Services;

/// <summary>
/// Сервис по работе с книгами
/// </summary>
public interface IBookService: IEntityService<Book>
{
    /// <summary>
    /// Создание книги
    /// </summary>
    Task<Guid> CreateBook(Book book);
    /// <summary>
    /// Редактирование книги
    /// </summary>
    Task<Guid> UpdateBook(Guid id, Book book);
    /// <summary>
    /// Перевод книги в архив 
    /// </summary>
    Task<ArchiveBookDto> ArchiveBook(Guid id);
    /// <summary>
    /// Получение списка книг
    /// </summary>
    Task<IReadOnlyList<Book>> GetBooks(BookFilterDto filter, int page, int pageSize);
    /// <summary>
    /// Добавление деталей книги
    /// </summary>
    Task<BookDetailsDto> AddBookDetails(Guid id, IFormFile coverImage, string description);
}