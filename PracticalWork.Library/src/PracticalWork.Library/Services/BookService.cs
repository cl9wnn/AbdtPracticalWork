using Microsoft.Extensions.Options;
using PracticalWork.Library.Abstractions.Services.Domain;
using PracticalWork.Library.Abstractions.Services.Infrastructure;
using PracticalWork.Library.Abstractions.Storage;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Enums;
using PracticalWork.Library.Exceptions;
using PracticalWork.Library.Models;
using PracticalWork.Library.Options.Cache;
using PracticalWork.Shared.Contracts.Events.Books;

namespace PracticalWork.Library.Services;

/// <summary>
/// Сервис по работе с книгами
/// </summary>
public sealed class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly ICacheService _cacheService;
    private readonly IFileStorageService _fileStorageService;
    private readonly IMessageBrokerProducer _kafkaProducer;
    private readonly IOptions<BooksCacheOptions> _cacheOptions;
    private readonly string _booksCacheVersionPrefix;

    public BookService(IBookRepository bookRepository, ICacheService cacheService,
        IFileStorageService fileStorageService, IOptions<BooksCacheOptions> options,
        IMessageBrokerProducer messageBrokerProducer)
    {
        _bookRepository = bookRepository;
        _cacheService = cacheService;
        _fileStorageService = fileStorageService;
        _cacheOptions = options;
        _kafkaProducer = messageBrokerProducer;

        _booksCacheVersionPrefix = options.Value.BooksCacheVersionPrefix;
    }

    /// <summary>
    /// Получение книги по ее идентификатору
    /// </summary>
    public async Task<Book> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _bookRepository.GetById(id, cancellationToken);
    }

    /// <summary>
    /// Проверка существования книги
    /// </summary>
    public async Task Exists(Guid id, CancellationToken cancellationToken)
    {
        var exists = await _bookRepository.Exists(id, cancellationToken);

        if (!exists)
        {
            throw new EntityNotFoundException("Книга не обнаружена!");
        }
    }

    /// <inheritdoc cref="IBookService.CreateBook"/>
    public async Task<Guid> CreateBook(Book book, CancellationToken cancellationToken)
    {
        book.Status = BookStatus.Available;

        var bookId = await _bookRepository.Add(book, cancellationToken);
        await _cacheService.IncrementVersionAsync(_booksCacheVersionPrefix, cancellationToken);

        var bookCreatedEvent = new BookCreatedEvent(
            BookId: bookId,
            Title: book.Title,
            Category: book.Category.ToString(),
            Authors: book.Authors.ToArray(),
            Year: book.Year,
            CreatedAt: DateTime.UtcNow
        );

        await _kafkaProducer.ProduceAsync(bookCreatedEvent.EventId.ToString(), bookCreatedEvent, cancellationToken);

        return bookId;
    }

    /// <inheritdoc cref="IBookService.UpdateBook"/>
    public async Task UpdateBook(Guid id, Book updatedBook, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetById(id, cancellationToken);

        if (book.IsArchived)
        {
            throw new BookServiceException("Книга в архиве!");
        }

        book.ChangeInformation(updatedBook.Title, updatedBook.Authors, updatedBook.Description, updatedBook.Year);
        await _bookRepository.Update(id, book, cancellationToken);

        await _cacheService.IncrementVersionAsync(_booksCacheVersionPrefix, cancellationToken);
    }

    /// <inheritdoc cref="IBookService.ArchiveBook"/>
    public async Task<ArchivedBookDto> ArchiveBook(Guid id, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetById(id, cancellationToken);
        book.Archive();

        await _bookRepository.Update(id, book, cancellationToken);
        await _cacheService.IncrementVersionAsync(_booksCacheVersionPrefix, cancellationToken);

        var bookArchivedEvent = new BookArchivedEvent(
             BookId: id,
             Title: book.Title,
             ArchivedAt: DateTime.UtcNow
        );

        await _kafkaProducer.ProduceAsync(bookArchivedEvent.EventId.ToString(), bookArchivedEvent, cancellationToken);
        
        return new ArchivedBookDto
        {
            Id = id,
            Title = book.Title,
            ArchivedAt = DateTime.UtcNow
        };
    }

    /// <inheritdoc cref="IBookService.ArchiveOldBooks"/>
    public async Task<ArchiveReportDto> ArchiveOldBooks(int yearsWithoutBorrow, int maxBooksPerRun, 
        CancellationToken cancellationToken)
    {
        var startTime = DateTime.UtcNow;
        var thresholdDate = startTime.AddYears(-yearsWithoutBorrow);
        
        var booksForArchiving = await _bookRepository.GetBooksForArchiving(thresholdDate,
            maxBooksPerRun, cancellationToken);
        
        var report = new ArchiveReportDto
        {
            TotalProcessed = booksForArchiving.Count
        };

        foreach (var (id, book) in booksForArchiving)
        {
            try
            {
                await ArchiveBook(id, cancellationToken);
                report.SuccessfullyArchived++;
            }
            catch (Exception ex)
            {
                report.Skipped++;

                report.SkippedDetails.Add(new ArchiveSkipDetailDto
                {
                    BookId = id,
                    Reason = $"Не удалось архивировать книгу \"{book.Title}\". Сообщение об ошибке {ex.Message}"
                });
            }
        }

        report.ExecutionTimeMs = (long)(DateTime.UtcNow - startTime).TotalMilliseconds;

        return report;
    }

    /// <inheritdoc cref="IBookService.GetBooksPage"/>
    public async Task<PageDto<BookListDto>> GetBooksPage(BookFilterDto filter, PaginationDto pagination,
        CancellationToken cancellationToken)
    {
        var keyPrefix = _cacheOptions.Value.BooksListCache.KeyPrefix;
        var ttlMinutes = _cacheOptions.Value.BooksListCache.TtlMinutes;
        var searchDto = new SearchBooksDto { BookFilter = filter, Pagination = pagination };

        var cachedBooks =
            await _cacheService.GetByModelAsync<SearchBooksDto, IReadOnlyList<BookListDto>>(keyPrefix,
                _booksCacheVersionPrefix, searchDto, cancellationToken);

        if (cachedBooks != null)
        {
            return new PageDto<BookListDto>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                Items = cachedBooks
            };
        }

        var books = await _bookRepository.GetBooks(filter, pagination, cancellationToken);
        await _cacheService.SetByModelAsync(keyPrefix, _booksCacheVersionPrefix, searchDto, books,
            TimeSpan.FromMinutes(ttlMinutes), cancellationToken);

        return new PageDto<BookListDto>
        {
            PageSize = pagination.PageSize,
            Page = pagination.Page,
            Items = books
        };
    }

    /// <inheritdoc cref="IBookService.AddBookDetails"/>
    public async Task AddBookDetails(Guid id, string description, Stream coverImageStream,
        string contentType, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetById(id, cancellationToken);

        var filePath = $"{DateTime.Today.Year}/{DateTime.Today.Month}/{id}{contentType}";
        book.UpdateDetails(description, filePath);

        await _fileStorageService.UploadFileAsync(filePath, coverImageStream, contentType, cancellationToken);
        await _bookRepository.Update(id, book, cancellationToken);
        await _cacheService.IncrementVersionAsync(_booksCacheVersionPrefix, cancellationToken);
    }
}