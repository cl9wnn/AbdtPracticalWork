using PracticalWork.Library.Abstractions.Services;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;

namespace PracticalWork.Library.Services;

public class LibraryService: ILibraryService
{
    public Task<Borrow> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Exists(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<LibraryBookDto>> GetLibraryBooks(BookFilterDto filter, int page, int pageSize)
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