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
    private readonly IOptions<BooksCacheOptions> _options;

    public BookService(IBookRepository bookRepository, ICacheService cacheService,
        IFileStorageService fileStorageService, IOptions<BooksCacheOptions> options)
    {
        _bookRepository = bookRepository;
        _cacheService = cacheService;
        _fileStorageService = fileStorageService;
        _options = options;
    }

    /// <summary> Получение книги по ее идентификатору </summary>
    public async Task<Book> GetById(Guid id)
    {
        try
        {
            return await _bookRepository.GetById(id);
        }
        catch (Exception ex)
        {
            throw new BookServiceException("Ошибка получения книги!", ex);
        }
    }

    /// <summary> Проверка существования книги </summary>
    public async Task Exists(Guid id)
    {
        
        var exists = await _bookRepository.Exists(id);

        if (!exists)
        {
            throw new BookServiceException("Книга не обнаружена!");
        }
        
    }

    /// <inheritdoc cref="IBookService.CreateBook"/>
    public async Task<Guid> CreateBook(Book book)
    {
        book.Status = BookStatus.Available;
        try
        {
            var bookId = await _bookRepository.Add(book);
            await _cacheService.IncrementVersionAsync(_options.Value.BooksListCache.KeyPrefix);

            return bookId;
        }
        catch (Exception ex)
        {
            throw new BookServiceException("Ошибка создание книги!", ex);
        }
    }

    /// <inheritdoc cref="IBookService.UpdateBook"/>
    public async Task UpdateBook(Guid id, Book updatedBook)
    {
        try
        {
            var book = await _bookRepository.GetById(id);

            if (book.IsArchived)
            {
                throw new BookServiceException("Книга в архиве!");
            }

            book.ChangeInformation(updatedBook.Title, updatedBook.Authors, updatedBook.Description, updatedBook.Year);
            await _bookRepository.Update(id, book);
            
            await _cacheService.IncrementVersionAsync(_options.Value.BooksListCache.KeyPrefix);
        }
        catch (NullReferenceException ex)
        {
            throw new BookServiceException("Книга не обнаружена!", ex);
        }
        catch (Exception ex)
        {
            throw new BookServiceException("Ошибка обновления книги!", ex);
        }
    }

    /// <inheritdoc cref="IBookService.ArchiveBook"/>
    public async Task<ArchivedBookDto> ArchiveBook(Guid id)
    {
        try
        {
            var book = await _bookRepository.GetById(id);
            book.Archive();
            await _bookRepository.Update(id, book);
            
            await _cacheService.IncrementVersionAsync(_options.Value.BooksListCache.KeyPrefix);
            
            return new ArchivedBookDto
            {
                Id = id,
                Title = book.Title,
                ArchivedAt = DateTime.UtcNow
            };
        }
        catch (NullReferenceException ex)
        {
            throw new BookServiceException("Книга не обнаружена!", ex);
        }
        catch (Exception ex)
        {
            throw new BookServiceException("Ошибка архивации книги!", ex);
        }
    }

    /// <inheritdoc cref="IBookService.GetBooksPage"/>
    public async Task<PageDto<BookListDto>> GetBooksPage(BookFilterDto filter, PaginationDto pagination)
    {
        try
        {
            var keyPrefix = _options.Value.BooksListCache.KeyPrefix;
            var ttlMinutes = _options.Value.BooksListCache.TtlMinutes;
            
            var cacheVersion = await _cacheService.GetVersionAsync(keyPrefix);
            var hash = CacheKeyHasher.GenerateCacheKey(keyPrefix, cacheVersion, new { filter, pagination });
            
            var cachedBooks = await _cacheService.GetAsync<IReadOnlyList<BookListDto>>(hash);

            if (cachedBooks != null)
            {
                return new PageDto<BookListDto>
                {
                    PageSize = pagination.PageSize,
                    Page = pagination.Page,
                    Items = cachedBooks
                };
            }
            
            var books = await _bookRepository.GetBooksPage(filter, pagination);
            await _cacheService.SetAsync(hash, books, TimeSpan.FromMinutes(ttlMinutes));
            
            return new PageDto<BookListDto>
            {
                PageSize = pagination.PageSize,
                Page = pagination.Page,
                Items = books
            };
        }
        catch (Exception ex)
        {
            throw new BookServiceException("Ошибка получения списка книг!", ex);
        }
    }

    /// <inheritdoc cref="IBookService.AddBookDetails"/>
    public async Task<BookDetailsDto> AddBookDetails(Guid id, string description, Stream coverImageStream,
        string contentType)
    {
        try
        {
            var book = await _bookRepository.GetById(id);

            var filePath = $"{DateTime.Today.Year}/{DateTime.Today.Month}/{id}{contentType}";

            var coverImagePath = await _fileStorageService.UploadFileAsync(filePath, coverImageStream, contentType);

            book.UpdateDetails(description, coverImagePath);
            await _bookRepository.Update(id, book);

            await _cacheService.IncrementVersionAsync(_options.Value.BooksListCache.KeyPrefix);
                
            return new BookDetailsDto
            {
                Id = id,
                Title = book.Title,
                Category = book.Category,
                Authors = book.Authors,
                Description = book.Description,
                Year = book.Year,
                CoverImagePath = coverImagePath,
                Status = book.Status,
                IsArchived = book.IsArchived
            };
        }
        catch (NullReferenceException ex)
        {
            throw new BookServiceException("Книга не обнаружена!", ex);
        }
        catch (Exception ex)
        {
            throw new BookServiceException("Ошибка добавления деталей книги!", ex);
        }
    }
}