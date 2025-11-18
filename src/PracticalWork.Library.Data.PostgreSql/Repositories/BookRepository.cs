using Microsoft.EntityFrameworkCore;
using PracticalWork.Library.Abstractions.Storage;
using PracticalWork.Library.Data.PostgreSql.Entities;
using PracticalWork.Library.Data.PostgreSql.Mappers.v1;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Enums;
using PracticalWork.Library.Models;

namespace PracticalWork.Library.Data.PostgreSql.Repositories;

public sealed class BookRepository : IBookRepository
{
    private readonly AppDbContext _appDbContext;

    public BookRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Book> GetById(Guid id)
    {
        var bookEntity = await _appDbContext.Books
            .FindAsync(id);

        if (bookEntity == null)
        {
            throw new KeyNotFoundException("Книга не найдена по данному идентификатору!");
        }

        return bookEntity.ToBook();
    }

    public async Task<Guid> Add(Book book)
    {
        AbstractBookEntity entity = book.Category switch
        {
            BookCategory.ScientificBook => new ScientificBookEntity(),
            BookCategory.EducationalBook => new EducationalBookEntity(),
            BookCategory.FictionBook => new FictionBookEntity(),
            _ => throw new ArgumentException($"Неподдерживаемая категория книги: {book.Category}",
                nameof(book.Category))
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

    public async Task<Book> Update(Guid id, Book book)
    {
        var bookEntity = await _appDbContext.Books
            .FindAsync(id);

        if (bookEntity == null)
        {
            throw new KeyNotFoundException("Книга не найдена по данному идентификатору!");
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

    public async Task Delete(Guid id)
    {
        var bookEntity = await _appDbContext.Books
            .FindAsync(id);

        if (bookEntity == null)
        {
            throw new KeyNotFoundException("Книга не найдена по данному идентификатору!");
        }

        _appDbContext.Books.Remove(bookEntity);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<ICollection<Book>> GetAll()
    {
        var bookEntities = await _appDbContext.Books
            .AsNoTracking()
            .ToListAsync();

        return bookEntities
            .Select(b => b.ToBook())
            .ToList();
    }

    public async Task<bool> Exists(Guid id)
    {
        return await _appDbContext.Books.AnyAsync(b => b.Id == id);
    }

    public async Task<IReadOnlyList<BookListDto>> GetBooksPage(BookFilterDto filter, PaginationDto pagination)
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
        
        var bookEntities = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .OrderBy(b => b.Title)
            .ToListAsync();

        return bookEntities
            .Select(b => b.ToBookListDto())
            .ToList();
    }
}