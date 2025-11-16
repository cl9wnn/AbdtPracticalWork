using PracticalWork.Library.Abstractions.Services.Domain;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;

namespace PracticalWork.Library.Services;

public class ReaderService: IReaderService
{
    public Task<Reader> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task Exists(Guid id)
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