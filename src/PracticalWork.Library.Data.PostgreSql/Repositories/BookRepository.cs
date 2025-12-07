using Microsoft.EntityFrameworkCore;
using PracticalWork.Library.Abstractions.Storage;
using PracticalWork.Library.Data.PostgreSql.Entities;
using PracticalWork.Library.Data.PostgreSql.Mappers.v1;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Enums;
using PracticalWork.Library.Exceptions;
using PracticalWork.Library.Models;
using PracticalWork.Library.SharedKernel.Abstractions;

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

    /// <inheritdoc cref="IEntityRepository{Guid,Book}.GetById"/>
    public async Task<Book> GetById(Guid id)
    {
        var bookEntity = await _appDbContext.Books
            .FindAsync(id);

        if (bookEntity == null)
        {
            throw new EntityNotFoundException("Книга не найдена по данному идентификатору!");
        }

        return bookEntity.ToBook();
    }
    
    /// <inheritdoc cref="IBookRepository.GetByTitle"/>
    public async Task<BookDetailsDto> GetByTitle(string title)
    {
        var bookEntity = await _appDbContext.Books
            .FirstOrDefaultAsync(b => b.Title == title);
        
        if (bookEntity == null)
        {
            throw new EntityNotFoundException("Книга не найдена по данному идентификатору!");
        }

        return bookEntity.ToBookDetailsDto();
    }

    /// <inheritdoc cref="IEntityRepository{Guid,Book}.Add"/>
    public async Task<Guid> Add(Book book)
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
        await _appDbContext.SaveChangesAsync();

        return entity.Id;
    }

    /// <inheritdoc cref="IEntityRepository{Guid,Book}.Update"/>
    public async Task<Book> Update(Guid id, Book book)
    {
        var bookEntity = await _appDbContext.Books
            .FindAsync(id);

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
        
        await _appDbContext.SaveChangesAsync();
        
        return bookEntity.ToBook();
    }

    /// <inheritdoc cref="IEntityRepository{Guid,Book}.Delete"/>
    public async Task Delete(Guid id)
    {
        var bookEntity = await _appDbContext.Books
            .FindAsync(id);

        if (bookEntity == null)
        {
            throw new EntityNotFoundException("Книга не найдена по данному идентификатору!");
        }

        _appDbContext.Books.Remove(bookEntity);
        await _appDbContext.SaveChangesAsync();
    }

    /// <inheritdoc cref="IEntityRepository{Guid,Book}.GetAll"/>
    public async Task<ICollection<Book>> GetAll()
    {
        var bookEntities = await _appDbContext.Books
            .Select(b => b.ToBook())
            .AsNoTracking()
            .ToListAsync();

        return bookEntities;
    }

    /// <inheritdoc cref="IEntityRepository{Guid,Book}.Exists"/>
    public async Task<bool> Exists(Guid id)
    {
        return await _appDbContext.Books.AnyAsync(b => b.Id == id);
    }

    /// <inheritdoc cref="IBookRepository.GetBooks"/>
    public async Task<IReadOnlyList<BookListDto>> GetBooks(BookFilterDto filter, PaginationDto pagination)
    {
        var query = BuildBooksQuery(filter, includeIssuance: false);
        
        return await query
            .OrderBy(b => b.Title)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(b => b.ToBookListDto())
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc cref="IBookRepository.GetLibraryBooks"/>
    public async Task<IReadOnlyList<LibraryBookDto>> GetLibraryBooks(BookFilterDto filter, PaginationDto pagination)
    {
        var query = BuildBooksQuery(filter, includeIssuance: true);

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
            .ToListAsync();

        return bookEntities
            .Select(x => x.Book.ToLibraryBookDto(x.ActiveBorrow))
            .ToList();
    }
    
    /// <summary>Построение запроса для поиска книг по фильтрации</summary>
    private IQueryable<AbstractBookEntity> BuildBooksQuery(BookFilterDto filter, bool includeIssuance)
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

        if (includeIssuance)
        {
            query = query
                .Where(b => b.Status != BookStatus.Archived)
                .Include(b => b.IssuanceRecords);
        }

        return query;
    }
}