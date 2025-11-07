using PracticalWork.Library.Abstractions.Services;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;

namespace PracticalWork.Library.Services;

public class ReaderService: IReaderService
{
    public Task<Reader> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> CreateReader(Reader reader)
    {
        throw new NotImplementedException();
    }

    public Task ExtendReader(Guid readerId, DateOnly newExpiryDate)
    {
        throw new NotImplementedException();
    }

    public Task CloseReader(Guid readerId)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<BorrowedBookDto>> GetBorrowedBooks(Guid readerId)
    {
        throw new NotImplementedException();
    }
}