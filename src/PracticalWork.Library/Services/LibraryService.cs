using PracticalWork.Library.Abstractions.Services.Domain;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;

namespace PracticalWork.Library.Services;

/// <summary>
/// Сервис по работе с библиотекой
/// </summary>
public class LibraryService: ILibraryService
{
    /// <inheritdoc cref="ILibraryService.GetLibraryBooksPage"/>
    public Task<PageDto<LibraryBookDto>> GetLibraryBooksPage(BookFilterDto filter, PaginationDto pagination)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ILibraryService.BorrowBook"/>
    public Task<Guid> BorrowBook(Guid bookId, Guid readerId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ILibraryService.ReturnBook"/>
    public Task ReturnBook(Guid bookId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ILibraryService.GetBookDetailsById"/>
    public Task<BookDetailsDto> GetBookDetailsById(Guid bookId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ILibraryService.GetBookDetailsByTitle"/>
    public Task<BookDetailsDto> GetBookDetailsByTitle(string title)
    {
        throw new NotImplementedException();
    }
}