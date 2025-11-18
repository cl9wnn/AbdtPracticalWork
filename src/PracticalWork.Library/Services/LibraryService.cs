using PracticalWork.Library.Abstractions.Services.Domain;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;

namespace PracticalWork.Library.Services;

public class LibraryService: ILibraryService
{
    public Task<Borrow> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task Exists(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<PageDto<LibraryBookDto>> GetLibraryBooksPage(BookFilterDto filter, PaginationDto pagination)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> BorrowBook(Guid bookId, Guid readerId)
    {
        throw new NotImplementedException();
    }

    public Task ReturnBook(Guid bookId)
    {
        throw new NotImplementedException();
    }

    public Task<BookDetailsDto> GetBookDetailsById(Guid bookId)
    {
        throw new NotImplementedException();
    }

    public Task<BookDetailsDto> GetBookDetailsByTitle(string title)
    {
        throw new NotImplementedException();
    }
}