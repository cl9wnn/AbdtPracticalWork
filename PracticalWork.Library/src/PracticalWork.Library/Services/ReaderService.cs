using Microsoft.Extensions.Options;
using PracticalWork.Library.Abstractions.Services.Domain;
using PracticalWork.Library.Abstractions.Services.Infrastructure;
using PracticalWork.Library.Abstractions.Storage;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Events.Readers;
using PracticalWork.Library.Exceptions;
using PracticalWork.Library.Models;
using PracticalWork.Library.Options;
using PracticalWork.Library.Options.Cache;

namespace PracticalWork.Library.Services;

/// <summary>
/// Сервис по работе с карточкой читателя
/// </summary>
public class ReaderService : IReaderService
{
    private readonly IReaderRepository _readerRepository;
    private readonly ICacheService _cacheService;
    private readonly IOptions<ReadersCacheOptions> _options;
    private readonly IMessageBrokerProducer _kafkaProducer;
    private readonly string _readersBooksVersionPrefix;

    public ReaderService(IReaderRepository readerRepository, ICacheService cacheService,
        IOptions<ReadersCacheOptions> options, IMessageBrokerProducer kafkaProducer)
    {
        _readerRepository = readerRepository;
        _cacheService = cacheService;
        _options = options;
        _kafkaProducer = kafkaProducer;
        _readersBooksVersionPrefix = options.Value.ReadersCacheVersionPrefix;
    }

    /// <summary>
    /// Получение карточки читателя по её идентификатору
    /// </summary>
    public async Task<Reader> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _readerRepository.GetById(id, cancellationToken);
    }

    /// <summary>
    /// Проверка существования карточки читателя
    /// </summary>
    public async Task Exists(Guid id, CancellationToken cancellationToken)
    {
        var exists = await _readerRepository.Exists(id, cancellationToken);

        if (!exists)
        {
            throw new EntityNotFoundException("Карточка читателя не обнаружена!");
        }
    }

    /// <inheritdoc cref="IReaderService.CreateReader"/>
    public async Task<Guid> CreateReader(Reader reader, CancellationToken cancellationToken)
    {
        reader.IsActive = true;

        var iExists = await _readerRepository.Exists(reader.PhoneNumber, cancellationToken);

        if (iExists)
        {
            throw new ReaderServiceException("Карточка по такому номеру телефона уже существует!");
        }
        
        var readerId = await _readerRepository.Add(reader, cancellationToken);

        var readerCreatedEvent = new ReaderCreatedEvent(
            ReaderId: readerId,
            FullName: reader.FullName,
            PhoneNumber: reader.PhoneNumber,
            Email: reader.Email,
            ExpiryDate: reader.ExpiryDate,
            CreatedAt: DateTime.UtcNow
        );
        
        await _kafkaProducer.ProduceAsync(readerCreatedEvent.EventId.ToString(), readerCreatedEvent, cancellationToken);
        
        return readerId;
    }

    /// <inheritdoc cref="IReaderService.ExtendReader"/>
    public async Task ExtendReader(Guid readerId, DateOnly newExpiryDate, CancellationToken cancellationToken)
    {
        var reader = await _readerRepository.GetById(readerId, cancellationToken);
        reader.Extend(newExpiryDate);
        await _readerRepository.Update(readerId, reader, cancellationToken);
    }

    /// <inheritdoc cref="IReaderService.CloseReader"/>
    public async Task CloseReader(Guid readerId, CancellationToken cancellationToken)
    {
        var reader = await _readerRepository.GetById(readerId, cancellationToken);
        var borrowedBooks = await _readerRepository.GetBorrowedBooks(readerId, cancellationToken);

        if (borrowedBooks.Any())
        {
            throw new ReaderServiceException("У пользователя есть взятые книги!", borrowedBooks);
        }

        reader.Close();

        await _readerRepository.Update(readerId, reader, cancellationToken);
        
        var readerClosedEvent = new ReaderClosedEvent(
            ReaderId: readerId,
            FullName: reader.FullName,
            ClosedAt: DateTime.UtcNow
        );
        
        await _kafkaProducer.ProduceAsync(readerClosedEvent.EventId.ToString(), readerClosedEvent, cancellationToken);
    }

    /// <inheritdoc cref="IReaderService.GetBorrowedBooks"/>
    public async Task<IReadOnlyList<BorrowedBookDto>> GetBorrowedBooks(Guid readerId, CancellationToken cancellationToken)
    {
        var keyPrefix = _options.Value.ReadersBooksCache.KeyPrefix;
        var ttlMinutes = _options.Value.ReadersBooksCache.TtlMinutes;

        var cachedBorrowedBooks = 
            await _cacheService.GetAsync<Guid, IReadOnlyList<BorrowedBookDto>>(keyPrefix, _readersBooksVersionPrefix,
            readerId, cancellationToken);

        if (cachedBorrowedBooks != null)
        {
            return cachedBorrowedBooks;
        }

        var borrowedBooks = await _readerRepository.GetBorrowedBooks(readerId, cancellationToken);
        await _cacheService.SetAsync(keyPrefix, _readersBooksVersionPrefix, readerId, borrowedBooks,
            TimeSpan.FromMinutes(ttlMinutes), cancellationToken);

        return borrowedBooks;
    }
}