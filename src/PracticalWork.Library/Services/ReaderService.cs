using PracticalWork.Library.Abstractions.Services.Domain;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Models;

namespace PracticalWork.Library.Services;

/// <summary>
/// Сервис по работе с карточкой читателя
/// </summary>
public class ReaderService: IReaderService
{
    /// <summary> Получение карточки читателя по её идентификатору </summary>
    public Task<Reader> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <summary> Проверка существования карточки читателя </summary>
    public Task Exists(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IReaderService.CreateReader"/>
    public Task<Guid> CreateReader(Reader reader)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IReaderService.ExtendReader"/>
    public Task ExtendReader(Guid readerId, DateOnly newExpiryDate)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IReaderService.CloseReader"/>
    public Task CloseReader(Guid readerId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IReaderService.GetBorrowedBooks"/>
    public Task<IReadOnlyList<BorrowedBookDto>> GetBorrowedBooks(Guid readerId)
    {
        throw new NotImplementedException();
    }
}