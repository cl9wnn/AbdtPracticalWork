using Microsoft.Extensions.Options;
using PracticalWork.Library.Abstractions.Services.Domain;
using PracticalWork.Library.Abstractions.Services.Infrastructure;
using PracticalWork.Library.Abstractions.Storage;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Exceptions;
using PracticalWork.Library.Models;
using PracticalWork.Library.Options.Cache;
using PracticalWork.Shared.Contracts.Events.Books;

namespace PracticalWork.Library.Services;

/// <summary>
/// Сервис по работе с библиотекой
/// </summary>
public class LibraryService : ILibraryService
{
    private readonly IBookRepository _bookRepository;
    private readonly IReaderRepository _readerRepository;
    private readonly IBookBorrowRepository _bookBorrowRepository;
    private readonly IOptions<BooksCacheOptions> _booksCacheOptions;
    private readonly ICacheService _cacheService;
    private readonly IFileStorageService _fileStorageService;
    private readonly IMessageBrokerProducer _kafkaProducer;
    private readonly string _booksCacheVersionPrefix;
    private readonly string _readersCacheVersionPrefix;


    public LibraryService(IBookRepository bookRepository, IReaderRepository readerRepository,
        IBookBorrowRepository bookBorrowRepository,
        IOptions<BooksCacheOptions> booksCacheOptions,
        IOptions<ReadersCacheOptions> readersCacheOptions,
        ICacheService cacheService,
        IFileStorageService fileStorageService, 
        IMessageBrokerProducer kafkaProducer)
    {
        _bookRepository = bookRepository;
        _readerRepository = readerRepository;
        _bookBorrowRepository = bookBorrowRepository;
        _booksCacheOptions = booksCacheOptions;
        _cacheService = cacheService;
        _fileStorageService = fileStorageService;
        _kafkaProducer = kafkaProducer;

        _readersCacheVersionPrefix = readersCacheOptions.Value.ReadersCacheVersionPrefix;
        _booksCacheVersionPrefix = _booksCacheOptions.Value.BooksCacheVersionPrefix;
    }

    /// <inheritdoc cref="ILibraryService.GetLibraryBooksPage"/>
    public async Task<PageDto<LibraryBookDto>> GetLibraryBooksPage(BookFilterDto filter, PaginationDto pagination,
        CancellationToken cancellationToken)
    {
        var keyPrefix = _booksCacheOptions.Value.LibraryBooksCache.KeyPrefix;
        var ttlMinutes = _booksCacheOptions.Value.LibraryBooksCache.TtlMinutes;
        var searchDto = new SearchBooksDto { BookFilter = filter, Pagination = pagination };

        var cachedBooks =
            await _cacheService.GetByModelAsync<SearchBooksDto, IReadOnlyList<LibraryBookDto>>(keyPrefix,
                _booksCacheVersionPrefix, searchDto, cancellationToken);

        if (cachedBooks != null)
        {
            return new PageDto<LibraryBookDto>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                Items = cachedBooks
            };
        }

        var books = await _bookRepository.GetLibraryBooks(filter, pagination, cancellationToken);
        await _cacheService.SetByModelAsync(keyPrefix, _booksCacheVersionPrefix, searchDto, books,
            TimeSpan.FromMinutes(ttlMinutes), cancellationToken);

        return new PageDto<LibraryBookDto>
        {
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            Items = books
        };
    }

    /// <inheritdoc cref="ILibraryService.BorrowBook"/>
    public async Task<Guid> BorrowBook(Guid bookId, Guid readerId, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetById(bookId, cancellationToken);
        var reader = await _readerRepository.GetById(readerId, cancellationToken);

        if (!reader.IsValid())
        {
            throw new LibraryServiceException("Карточка читателя не активна!");
        }

        book.Borrow();
        var borrow = Borrow.Create();

        var borrowBookId = await _bookBorrowRepository.Create(bookId, readerId, borrow, cancellationToken);
        await _bookRepository.Update(bookId, book, cancellationToken);

        await _cacheService.IncrementVersionAsync(_booksCacheVersionPrefix, cancellationToken);
        await _cacheService.IncrementVersionAsync(_readersCacheVersionPrefix, cancellationToken);

        var bookBorrowedEvent = new BookBorrowedEvent(
            BookId: bookId,
            ReaderId: readerId,
            BookTitle: book.Title,
            ReaderName: reader.FullName,
            BorrowDate: borrow.BorrowDate,
            DueDate: borrow.DueDate
        );

        await _kafkaProducer.ProduceAsync(bookBorrowedEvent.EventId.ToString(), bookBorrowedEvent, cancellationToken);

        return borrowBookId;
    }

    /// <inheritdoc cref="ILibraryService.ReturnBook"/>
    public async Task ReturnBook(Guid bookId, CancellationToken cancellationToken)
    {
        var borrow = await _bookBorrowRepository.GetActiveBorrowByBookId(bookId, cancellationToken);
        var reader = await _bookBorrowRepository.GetReaderInfoByBorrowedBookId(bookId, cancellationToken);
        var book = await _bookRepository.GetById(bookId, cancellationToken);

        borrow.ReturnBook();
        book.Return();

        await _bookBorrowRepository.Update(bookId, borrow, cancellationToken);
        await _bookRepository.Update(bookId, book, cancellationToken);

        await _cacheService.IncrementVersionAsync(_booksCacheVersionPrefix, cancellationToken);
        await _cacheService.IncrementVersionAsync(_readersCacheVersionPrefix, cancellationToken);

        var bookReturnedEvent = new BookReturnedEvent(
             BookId: bookId,
             ReaderId :reader.Id,
             BookTitle: book.Title,
             ReaderName: reader.FullName,
             ReturnDate: borrow.ReturnDate
        );
        
        await _kafkaProducer.ProduceAsync(bookReturnedEvent.EventId.ToString(), bookReturnedEvent, cancellationToken);
    }

    /// <inheritdoc cref="ILibraryService.GetBookDetailsById"/>
    public async Task<BookDetailsDto> GetBookDetailsById(Guid bookId, CancellationToken cancellationToken)
    {
        var keyPrefix = _booksCacheOptions.Value.BookDetailsCache.KeyPrefix;
        var ttlMinutes = _booksCacheOptions.Value.BookDetailsCache.TtlMinutes;

        var cachedBook =
            await _cacheService.GetAsync<Guid, BookDetailsDto>(keyPrefix, _booksCacheVersionPrefix, bookId, cancellationToken);

        if (cachedBook != null)
        {
            return cachedBook;
        }

        var book = await _bookRepository.GetById(bookId, cancellationToken);
        var coverImagePathUrl = await _fileStorageService.GetFilePathAsync(book.CoverImagePath, cancellationToken);

        var bookDetails = new BookDetailsDto
        {
            Id = bookId,
            Title = book.Title,
            Category = book.Category,
            Authors = book.Authors,
            Description = book.Description,
            Year = book.Year,
            CoverImagePath = coverImagePathUrl,
            Status = book.Status,
            IsArchived = book.IsArchived,
        };

        await _cacheService.SetAsync(keyPrefix, _booksCacheVersionPrefix, bookId, bookDetails,
            TimeSpan.FromMinutes(ttlMinutes), cancellationToken);

        return bookDetails;
    }

    /// <inheritdoc cref="ILibraryService.GetBookDetailsByTitle"/>
    public async Task<BookDetailsDto> GetBookDetailsByTitle(string title, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByTitle(title, cancellationToken);
        var coverImagePathUrl = await _fileStorageService.GetFilePathAsync(book.CoverImagePath, cancellationToken);
        book.CoverImagePath = coverImagePathUrl;

        return book;
    }
}