using Microsoft.Extensions.Options;
using PracticalWork.Library.Abstractions.Services.Domain;
using PracticalWork.Library.Abstractions.Services.Infrastructure;
using PracticalWork.Library.Abstractions.Storage;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Enums;
using PracticalWork.Library.Exceptions;
using PracticalWork.Library.Models;
using PracticalWork.Library.Options;
using PracticalWork.Library.SharedKernel.Helpers;

namespace PracticalWork.Library.Services;

/// <summary>
/// Сервис по работе с книгами
/// </summary>
public sealed class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly ICacheService _cacheService;
    private readonly IFileStorageService _fileStorageService;
    private readonly IOptions<BooksCacheOptions> _cacheOptions;
    private readonly string _booksCacheVersionPrefix;

    public BookService(IBookRepository bookRepository, ICacheService cacheService,
        IFileStorageService fileStorageService, IOptions<BooksCacheOptions> options)
    {
        _bookRepository = bookRepository;
        _cacheService = cacheService;
        _fileStorageService = fileStorageService;
        _cacheOptions = options;

        _booksCacheVersionPrefix = options.Value.BooksCacheVersionPrefix;
    }

    /// <summary> Получение книги по ее идентификатору </summary>
    public async Task<Book> GetById(Guid id)
    {
        return await _bookRepository.GetById(id);
    }

    /// <summary> Проверка существования книги </summary>
    public async Task Exists(Guid id)
    {
        var exists = await _bookRepository.Exists(id);

        if (!exists)
        {
            throw new EntityNotFoundException("Книга не обнаружена!");
        }
    }

    /// <inheritdoc cref="IBookService.CreateBook"/>
    public async Task<Guid> CreateBook(Book book)
    {
        book.Status = BookStatus.Available;

        var bookId = await _bookRepository.Add(book);
        await _cacheService.IncrementVersionAsync(_booksCacheVersionPrefix);

        return bookId;
    }

    /// <inheritdoc cref="IBookService.UpdateBook"/>
    public async Task UpdateBook(Guid id, Book updatedBook)
    {
        var book = await _bookRepository.GetById(id);

        if (book.IsArchived)
        {
            throw new BookServiceException("Книга в архиве!");
        }

        book.ChangeInformation(updatedBook.Title, updatedBook.Authors, updatedBook.Description, updatedBook.Year);
        await _bookRepository.Update(id, book);

        await _cacheService.IncrementVersionAsync(_booksCacheVersionPrefix);
    }

    /// <inheritdoc cref="IBookService.ArchiveBook"/>
    public async Task<ArchivedBookDto> ArchiveBook(Guid id)
    {
        var book = await _bookRepository.GetById(id);
        book.Archive();

        await _bookRepository.Update(id, book);
        await _cacheService.IncrementVersionAsync(_booksCacheVersionPrefix);

        return new ArchivedBookDto
        {
            Id = id,
            Title = book.Title,
            ArchivedAt = DateTime.UtcNow
        };
    }

    /// <inheritdoc cref="IBookService.GetBooksPage"/>
    public async Task<PageDto<BookListDto>> GetBooksPage(BookFilterDto filter, PaginationDto pagination)
    {
        var keyPrefix = _cacheOptions.Value.BooksListCache.KeyPrefix;
        var ttlMinutes = _cacheOptions.Value.BooksListCache.TtlMinutes;

        var cacheVersion = await _cacheService.GetVersionAsync(_booksCacheVersionPrefix);
        var hashedKey = CacheKeyHasher.GenerateCacheKey(keyPrefix, cacheVersion, new { filter, pagination });

        var cachedBooks = await _cacheService.GetAsync<IReadOnlyList<BookListDto>>(hashedKey);

        if (cachedBooks != null)
        {
            return new PageDto<BookListDto>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                Items = cachedBooks
            };
        }

        var books = await _bookRepository.GetBooks(filter, pagination);
        await _cacheService.SetAsync(hashedKey, books, TimeSpan.FromMinutes(ttlMinutes));

        return new PageDto<BookListDto>
        {
            PageSize = pagination.PageSize,
            Page = pagination.Page,
            Items = books
        };
    }

    /// <inheritdoc cref="IBookService.AddBookDetails"/>
    public async Task AddBookDetails(Guid id, string description, Stream coverImageStream,
        string contentType)
    {
        var book = await _bookRepository.GetById(id);

        var filePath = $"{DateTime.Today.Year}/{DateTime.Today.Month}/{id}{contentType}";
        book.UpdateDetails(description, filePath);

        await _fileStorageService.UploadFileAsync(filePath, coverImageStream, contentType);
        await _bookRepository.Update(id, book);
        await _cacheService.IncrementVersionAsync(_booksCacheVersionPrefix);
    }
}