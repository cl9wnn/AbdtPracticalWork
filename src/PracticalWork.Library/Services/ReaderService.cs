using Microsoft.Extensions.Options;
using PracticalWork.Library.Abstractions.Services.Domain;
using PracticalWork.Library.Abstractions.Services.Infrastructure;
using PracticalWork.Library.Abstractions.Storage;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Exceptions;
using PracticalWork.Library.Models;
using PracticalWork.Library.Options;

namespace PracticalWork.Library.Services;

/// <summary>
/// Сервис по работе с карточкой читателя
/// </summary>
public class ReaderService : IReaderService
{
    private readonly IReaderRepository _readerRepository;
    private readonly ICacheService _cacheService;
    private readonly IOptions<ReadersCacheOptions> _options;
    private readonly string _readersBooksVersionPrefix;

    public ReaderService(IReaderRepository readerRepository, ICacheService cacheService,
        IOptions<ReadersCacheOptions> options)
    {
        _readerRepository = readerRepository;
        _cacheService = cacheService;
        _options = options;
        _readersBooksVersionPrefix = options.Value.ReadersCacheVersionPrefix;
    }

    /// <summary> Получение карточки читателя по её идентификатору </summary>
    public async Task<Reader> GetById(Guid id)
    {
        return await _readerRepository.GetById(id);
    }

    /// <summary> Проверка существования карточки читателя </summary>
    public async Task Exists(Guid id)
    {
        var exists = await _readerRepository.Exists(id);

        if (!exists)
        {
            throw new EntityNotFoundException("Карточка читателя не обнаружена!");
        }
    }

    /// <inheritdoc cref="IReaderService.CreateReader"/>
    public async Task<Guid> CreateReader(Reader reader)
    {
        reader.IsActive = true;

        var iExists = await _readerRepository.Exists(reader.PhoneNumber);

        if (iExists)
        {
            throw new ReaderServiceException("Карточка по такому номеру телефона уже существует!");
        }

        return await _readerRepository.Add(reader);
    }

    /// <inheritdoc cref="IReaderService.ExtendReader"/>
    public async Task ExtendReader(Guid readerId, DateOnly newExpiryDate)
    {
        var reader = await _readerRepository.GetById(readerId);
        reader.Extend(newExpiryDate);
        await _readerRepository.Update(readerId, reader);
    }

    /// <inheritdoc cref="IReaderService.CloseReader"/>
    public async Task CloseReader(Guid readerId)
    {
        var reader = await _readerRepository.GetById(readerId);
        var borrowedBooks = await _readerRepository.GetBorrowedBooks(readerId);

        if (borrowedBooks.Any())
        {
            throw new ReaderServiceException("У пользователя есть взятые книги!", borrowedBooks);
        }

        reader.Close();

        await _readerRepository.Update(readerId, reader);
    }

    /// <inheritdoc cref="IReaderService.GetBorrowedBooks"/>
    public async Task<IReadOnlyList<BorrowedBookDto>> GetBorrowedBooks(Guid readerId)
    {
        var keyPrefix = _options.Value.ReadersBooksCache.KeyPrefix;
        var ttlMinutes = _options.Value.ReadersBooksCache.TtlMinutes;

        var cachedBorrowedBooks = 
            await _cacheService.GetAsync<Guid, IReadOnlyList<BorrowedBookDto>>(keyPrefix, _readersBooksVersionPrefix,
            readerId);

        if (cachedBorrowedBooks != null)
        {
            return cachedBorrowedBooks;
        }

        var borrowedBooks = await _readerRepository.GetBorrowedBooks(readerId);
        await _cacheService.SetAsync(keyPrefix, _readersBooksVersionPrefix, readerId, borrowedBooks,
            TimeSpan.FromMinutes(ttlMinutes));

        return borrowedBooks;
    }
}