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
    private readonly string _booksCacheVersionPrefix;
    private readonly string _readersCacheVersionPrefix;


    public LibraryService(IBookRepository bookRepository, IReaderRepository readerRepository,
        IBookBorrowRepository bookBorrowRepository,
        IOptions<BooksCacheOptions> booksCacheOptions,
        IOptions<ReadersCacheOptions> readersCacheOptions,
        ICacheService cacheService,
        IFileStorageService fileStorageService)
    {
        _bookRepository = bookRepository;
        _readerRepository = readerRepository;
        _bookBorrowRepository = bookBorrowRepository;
        _booksCacheOptions = booksCacheOptions;
        _cacheService = cacheService;
        _fileStorageService = fileStorageService;
        
        _readersCacheVersionPrefix = readersCacheOptions.Value.ReadersCacheVersionPrefix;
        _booksCacheVersionPrefix = _booksCacheOptions.Value.BooksCacheVersionPrefix;
    }

    /// <inheritdoc cref="ILibraryService.GetLibraryBooksPage"/>
    public async Task<PageDto<LibraryBookDto>> GetLibraryBooksPage(BookFilterDto filter, PaginationDto pagination)
    {
        try
        {
            var keyPrefix = _booksCacheOptions.Value.LibraryBooksCache.KeyPrefix;
            var ttlMinutes = _booksCacheOptions.Value.LibraryBooksCache.TtlMinutes;
            
            var cacheVersion = await _cacheService.GetVersionAsync(_booksCacheVersionPrefix);
            var hashedKey =
                CacheKeyHasher.GenerateCacheKey(keyPrefix, cacheVersion, new { filter, pagination });

            var cachedBooks = await _cacheService.GetAsync<IReadOnlyList<LibraryBookDto>>(hashedKey);

            if (cachedBooks != null)
            {
                return new PageDto<LibraryBookDto>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    Items = cachedBooks
                };
            }

            var books = await _bookRepository.GetLibraryBooks(filter, pagination);
            await _cacheService.SetAsync(hashedKey, books, TimeSpan.FromMinutes(ttlMinutes));

            return new PageDto<LibraryBookDto>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                Items = books
            };
        }
        catch (Exception ex)
        {
            throw new LibraryServiceException("Ошибка получения списка книг из библиотеки!", ex);
        }
    }

    /// <inheritdoc cref="ILibraryService.BorrowBook"/>
    public async Task<Guid> BorrowBook(Guid bookId, Guid readerId)
    {
        try
        {
            var book = await _bookRepository.GetById(bookId);
            var reader = await _readerRepository.GetById(readerId);

            if (!reader.IsValid())
            {
                throw new LibraryServiceException("Карточка читателя не активна!");
            }

            book.Borrow();
            var borrow = Borrow.Create();

            var borrowBookId = await _bookBorrowRepository.Create(bookId, readerId, borrow);
            await _bookRepository.Update(bookId, book);
            
            await _cacheService.IncrementVersionAsync(_booksCacheVersionPrefix);
            await _cacheService.IncrementVersionAsync(_readersCacheVersionPrefix);
            
            return borrowBookId;
        }
        catch (EntityNotFoundException ex)
        {
            throw new LibraryServiceException("Книга или карточка не обнаружена!", ex);
        }
        catch (Exception ex)
        {
            throw new LibraryServiceException("Ошибка выдачи книги читателю!", ex);
        }
    }

    /// <inheritdoc cref="ILibraryService.ReturnBook"/>
    public async Task ReturnBook(Guid bookId)
    {
        try
        {
            var borrow = await _bookBorrowRepository.GetActiveBorrowByBookId(bookId);
            var book = await _bookRepository.GetById(bookId);
            
            borrow.ReturnBook();
            book.Return();
            
            await _bookBorrowRepository.Update(bookId, borrow);
            await _bookRepository.Update(bookId, book);
            
            await _cacheService.IncrementVersionAsync(_booksCacheVersionPrefix);
            await _cacheService.IncrementVersionAsync(_readersCacheVersionPrefix);
        }
        catch (EntityNotFoundException ex)
        {
            throw new LibraryServiceException("Выдача книги не обнаружена!", ex);
        }
        catch (Exception ex)
        {
            throw new LibraryServiceException("Ошибка возврата книги!", ex);
        }
    }

    /// <inheritdoc cref="ILibraryService.GetBookDetailsById"/>
    public async Task<BookDetailsDto> GetBookDetailsById(Guid bookId)
    {
        try
        {
            var keyPrefix = _booksCacheOptions.Value.BookDetailsCache.KeyPrefix;
            var ttlMinutes = _booksCacheOptions.Value.BookDetailsCache.TtlMinutes;
            
            var cacheVersion = await _cacheService.GetVersionAsync(_booksCacheVersionPrefix);
            var cacheKey = $"{keyPrefix}:v{cacheVersion}:{bookId}";

            var cachedBook = await _cacheService.GetAsync<BookDetailsDto>(cacheKey);

            if (cachedBook != null)
            {
                return cachedBook;
            }

            var book = await _bookRepository.GetById(bookId);
            var coverImagePathUrl = await _fileStorageService.GetFilePathAsync(book.CoverImagePath);
            
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
            
            await _cacheService.SetAsync(cacheKey, bookDetails, TimeSpan.FromMinutes(ttlMinutes));

            return bookDetails;
        }
        catch (EntityNotFoundException ex)
        {
            throw new LibraryServiceException("Книга не обнаружена!", ex);
        }
        catch (Exception ex)
        {
            throw new LibraryServiceException("Ошибка получения деталей книги!", ex);
        }
    }

    /// <inheritdoc cref="ILibraryService.GetBookDetailsByTitle"/>
    public async Task<BookDetailsDto> GetBookDetailsByTitle(string title)
    {
        try
        {
            var book = await _bookRepository.GetByTitle(title);
            var coverImagePathUrl = await _fileStorageService.GetFilePathAsync(book.CoverImagePath);
            book.CoverImagePath = coverImagePathUrl;

            return book;
        }
        catch (EntityNotFoundException ex)
        {
            throw new LibraryServiceException("Книга не обнаружена!", ex);
        }
        catch (Exception ex)
        {
            throw new LibraryServiceException("Ошибка получения деталей книги!", ex);
        }
    }
}