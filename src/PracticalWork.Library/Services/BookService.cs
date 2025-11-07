using Microsoft.AspNetCore.Http;
using PracticalWork.Library.Abstractions.Services;
using PracticalWork.Library.Abstractions.Storage;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Enums;
using PracticalWork.Library.Exceptions;
using PracticalWork.Library.Models;

namespace PracticalWork.Library.Services;

public sealed class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Guid> CreateBook(Book book)
    {
        book.Status = BookStatus.Available;
        try
        {
            return await _bookRepository.AddAsync(book);
        }
        catch (Exception ex)
        {
            throw new BookServiceException("Ошибка создание книги!", ex);
        }
    }

    public Task UpdateBook(Guid id, Book book)
    {
        throw new NotImplementedException();
    }

    public Task<ArchiveBookDto> ArchiveBook(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Book>> GetBooks(BookFilterDto filter, int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<BookDetailsDto> AddBookDetails(Guid id, IFormFile coverImage, string description)
    {
        throw new NotImplementedException();
    }

    public Task<Book> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}