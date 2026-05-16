using Microsoft.EntityFrameworkCore;
using PracticalWork.Library.Abstractions.Storage;
using PracticalWork.Library.Data.PostgreSql.Entities;
using PracticalWork.Library.Data.PostgreSql.Mappers.v1;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Enums;
using PracticalWork.Library.Exceptions;
using PracticalWork.Library.Models;
using PracticalWork.Shared.Abstractions.Interfaces;

namespace PracticalWork.Library.Data.PostgreSql.Repositories;

/// <summary>
/// Репозиторий для работы с книгами
/// </summary>
public sealed class BookRepository : IBookRepository
{
    private readonly AppDbContext _appDbContext;

    public BookRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    /// <inheritdoc cref="IEntityRepository{TKey,TDto}.GetById"/>
    public async Task<Book> GetById(Guid id, CancellationToken cancellationToken)
    {
        var bookEntity = await _appDbContext.Books
            .FindAsync([id], cancellationToken);

        if (bookEntity == null)
        {
            throw new EntityNotFoundException("Книга не найдена по данному идентификатору!");
        }

        return bookEntity.ToBook();
    }
    
    /// <inheritdoc cref="IBookRepository.GetByTitle"/>
    public async Task<BookDetailsDto> GetByTitle(string title, CancellationToken cancellationToken)
    {
        var bookEntity = await _appDbContext.Books
            .FirstOrDefaultAsync(b => b.Title == title, cancellationToken: cancellationToken);
        
        if (bookEntity == null)
        {
            throw new EntityNotFoundException("Книга не найдена по данному идентификатору!");
        }

        return bookEntity.ToBookDetailsDto();
    }

    /// <inheritdoc cref="IEntityRepository{Guid,Book}.Add"/>
    public async Task<Guid> Add(Book book, CancellationToken cancellationToken)
    {
        AbstractBookEntity entity = book.Category switch
        {
            BookCategory.ScientificBook => new ScientificBookEntity(),
            BookCategory.EducationalBook => new EducationalBookEntity(),
            BookCategory.FictionBook => new FictionBookEntity(),
            _ => throw new BookServiceException($"Неподдерживаемая категория книги: {book.Category}")
        };

        entity.Title = book.Title;
        entity.Description = book.Description;
        entity.Year = book.Year;
        entity.Authors = book.Authors;
        entity.Status = book.Status;
        entity.CreatedAt = DateTime.UtcNow;

        _appDbContext.Add(entity);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    /// <inheritdoc cref="IEntityRepository{Guid,Book}.Update"/>
    public async Task<Book> Update(Guid id, Book book, CancellationToken cancellationToken)
    {
        var bookEntity = await _appDbContext.Books
            .FindAsync([id], cancellationToken);

        if (bookEntity == null)
        {
            throw new EntityNotFoundException("Книга не найдена по данному идентификатору!");
        }
        
        bookEntity.Title = book.Title;
        bookEntity.Description = book.Description;
        bookEntity.Year = book.Year;
        bookEntity.Authors = book.Authors;
        bookEntity.Status = book.Status;
        bookEntity.UpdatedAt = DateTime.UtcNow;

        if (book.CoverImagePath != null)
        {
            bookEntity.CoverImagePath = book.CoverImagePath;
        }
        
        await _appDbContext.SaveChangesAsync(cancellationToken);
        
        return bookEntity.ToBook();
    }

    /// <inheritdoc cref="IEntityRepository{Guid,Book}.Delete"/>
    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        var bookEntity = await _appDbContext.Books
            .FindAsync([id], cancellationToken);

        if (bookEntity == null)
        {
            throw new EntityNotFoundException("Книга не найдена по данному идентификатору!");
        }

        _appDbContext.Books.Remove(bookEntity);
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc cref="IEntityRepository{Guid,Book}.GetAll"/>
    public async Task<ICollection<Book>> GetAll(CancellationToken cancellationToken)
    {
        var bookEntities = await _appDbContext.Books
            .Select(b => b.ToBook())
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);

        return bookEntities;
    }

    /// <inheritdoc cref="IEntityRepository{Guid,Book}.Exists"/>
    public async Task<bool> Exists(Guid id, CancellationToken cancellationToken)
    {
        return await _appDbContext.Books.AnyAsync(b => b.Id == id, cancellationToken: cancellationToken);
    }

    /// <inheritdoc cref="IBookRepository.GetBooks"/>
    public async Task<IReadOnlyList<BookListDto>> GetBooks(BookFilterDto filter, PaginationDto pagination,
        CancellationToken cancellationToken)
    {
        var query = BuildBooksQuery(filter);
        
        return await query
            .OrderBy(b => b.Title)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .AsNoTracking()
            .AsNoTracking()
            .Select(b => new BookListDto
            {
                Title = b.Title,
                Authors = b.Authors,
                Description = b.Description,
                Year = b.Year
            })
            .ToListAsync(cancellationToken: cancellationToken);
    }

    /// <inheritdoc cref="IBookRepository.GetNewBooksCount"/>
    public async Task<int> GetNewBooksCount(DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.Books
            .CountAsync(x =>
                    x.CreatedAt >= from &&
                    x.CreatedAt <= to,
                cancellationToken);
    }

    /// <inheritdoc cref="IBookRepository.GetLibraryBooks"/>
    public async Task<IReadOnlyList<LibraryBookDto>> GetLibraryBooks(BookFilterDto filter, PaginationDto pagination,
        CancellationToken cancellationToken)
    {
        var query = BuildBooksQuery(filter);

        var bookEntities = await query
            .OrderBy(b => b.Title)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(b => new 
            {
                Book = b,
                ActiveBorrow = b.IssuanceRecords
                    .FirstOrDefault(r => r.Status == BookIssueStatus.Issued) 
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);

        return bookEntities
            .Select(x => x.Book.ToLibraryBookDto(x.ActiveBorrow))
            .ToList();
    }

    /// <inheritdoc cref="IBookRepository.GetBooksForArchiving"/>
    public async Task<IReadOnlyList<(Guid, Book)>> GetBooksForArchiving(DateTime thresholdDate, int limit, 
        CancellationToken cancellationToken)
    {
        return await _appDbContext.Books
            .Where(b =>
                b.Status == BookStatus.Available &&
                (
                    !b.IssuanceRecords.Any() ||
                    b.IssuanceRecords
                        .Max(r => r.BorrowDate) < DateOnly.FromDateTime(thresholdDate)
                )
            )
            .OrderBy(b => b.CreatedAt)
            .Take(limit)
            .Select(b => new ValueTuple<Guid, Book>(
                b.Id,
                b.ToBook()
            ))
            .ToListAsync(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Построение запроса для поиска книг по фильтрации
    /// </summary>
    private IQueryable<AbstractBookEntity> BuildBooksQuery(BookFilterDto filter)
    {
        IQueryable<AbstractBookEntity> query = filter.Category switch
        {
            BookCategory.ScientificBook => _appDbContext.ScientificBooks,
            BookCategory.EducationalBook => _appDbContext.EducationalBooks,
            BookCategory.FictionBook => _appDbContext.FictionBooks,
            _ => _appDbContext.Books
        };

        if (!string.IsNullOrWhiteSpace(filter.Author))
            query = query.Where(b => b.Authors.Contains(filter.Author));

        if (filter.Status != null)
            query = query.Where(b => b.Status == filter.Status);

        if (filter.AvailableOnly == true)
            query = query.Where(b => b.Status == BookStatus.Available);

        return query;
    }
}