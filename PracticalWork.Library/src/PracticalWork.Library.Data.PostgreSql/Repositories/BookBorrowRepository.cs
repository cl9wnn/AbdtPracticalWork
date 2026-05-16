using Microsoft.EntityFrameworkCore;
using PracticalWork.Library.Abstractions.Storage;
using PracticalWork.Library.Data.PostgreSql.Entities;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Enums;
using PracticalWork.Library.Exceptions;
using PracticalWork.Library.Models;

namespace PracticalWork.Library.Data.PostgreSql.Repositories;

/// <summary>
/// Репозиторий для работы с записями о выдаче книг читателям
/// </summary>
public class BookBorrowRepository: IBookBorrowRepository
{
    private readonly AppDbContext _dbContext;

    public BookBorrowRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    /// <inheritdoc cref="IBookBorrowRepository.Create"/>
    public async Task<Guid> Create(Guid bookId, Guid readerId, Borrow bookBorrow, CancellationToken cancellationToken)
    {
        var bookBorrowEntity = new BookBorrowEntity
        {
            CreatedAt = DateTime.UtcNow,
            BookId = bookId,
            ReaderId = readerId,
            BorrowDate = bookBorrow.BorrowDate,
            DueDate = bookBorrow.DueDate,
            Status = BookIssueStatus.Issued
        };
        
        await _dbContext.BookBorrows
            .AddAsync(bookBorrowEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return bookBorrowEntity.Id;
    }

    /// <inheritdoc cref="IBookBorrowRepository.GetActiveBorrowByBookId"/>
    public async Task<Borrow> GetActiveBorrowByBookId(Guid bookId, CancellationToken cancellationToken)
    {
        var bookBorrowEntity = await _dbContext.BookBorrows
            .FirstOrDefaultAsync(b => b.BookId == bookId && b.Status == BookIssueStatus.Issued,
                cancellationToken: cancellationToken);

        if (bookBorrowEntity == null)
        {
            throw new EntityNotFoundException("Отсутствует активная запись о выдаче книги!");
        }
        
        return new Borrow
        {
            BorrowDate = bookBorrowEntity.BorrowDate,
            DueDate = bookBorrowEntity.DueDate,
            ReturnDate = bookBorrowEntity.ReturnDate,
            Status = bookBorrowEntity.Status
        };
    }

    /// <inheritdoc cref="IBookBorrowRepository.Update"/>
    public async Task Update(Guid bookId, Borrow bookBorrow, CancellationToken cancellationToken)
    {
        var bookBorrowEntity = await _dbContext.BookBorrows
            .FirstOrDefaultAsync(b => b.BookId == bookId && b.Status == BookIssueStatus.Issued,
                cancellationToken: cancellationToken);
        
        if (bookBorrow == null)
        {
            throw new EntityNotFoundException("Отсутствует активная запись о выдаче книги!");
        }
        
        bookBorrowEntity.ReturnDate = bookBorrow.ReturnDate;
        bookBorrowEntity.Status = bookBorrow.Status;
        bookBorrowEntity.UpdatedAt = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc cref="IBookBorrowRepository.GetReaderInfoByBorrowedBookId"/>
    public async Task<ReaderInfoDto> GetReaderInfoByBorrowedBookId(Guid bookId, CancellationToken cancellationToken)
    {
        var readerInfo = await _dbContext.BookBorrows
            .Where(b => b.BookId == bookId && b.Status == BookIssueStatus.Issued)
            .Select(b => new ReaderInfoDto
            {
                Id = b.Reader.Id,
                FullName = b.Reader.FullName,
                PhoneNumber = b.Reader.PhoneNumber,
                Email = b.Reader.Email
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (readerInfo == null)
        {
            throw new EntityNotFoundException("Отсутствует активная запись о выдаче книги!");
        }

        return readerInfo;
    }
    
    /// <inheritdoc cref="IBookBorrowRepository.GetBorrowsDueInDays"/>
    public async Task<IReadOnlyList<BorrowedBookDto>> GetBorrowsDueInDays(int days, CancellationToken cancellationToken)
    {
        var targetDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(days);
        
        return await _dbContext.BookBorrows
            .Where(x =>
                x.Status == BookIssueStatus.Issued &&
                x.DueDate == targetDate)
            .Select(x => new BorrowedBookDto
            {
                BookId = x.BookId,
                Title = x.Book.Title,
                Authors = x.Book.Authors,
                Description = x.Book.Description,
                Year = x.Book.Year,
                BorrowDate = x.BorrowDate,
                DueDate = x.DueDate,
            })            
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc cref="IBookBorrowRepository.GetBorrowedBooksCount"/>
    public async Task<int> GetBorrowedBooksCount(DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
    {
        return await _dbContext.BookBorrows
            .CountAsync(x =>
                    x.BorrowDate >= from &&
                    x.BorrowDate <= to,
                cancellationToken);
    }

    /// <inheritdoc cref="IBookBorrowRepository.GetReturnedBooksCount"/>
    public async Task<int> GetReturnedBooksCount(DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
    {
        return await _dbContext.BookBorrows
            .CountAsync(x =>
                    x.ReturnDate != null &&
                    x.ReturnDate >= from &&
                    x.ReturnDate <= to,
                cancellationToken);
    }

    /// <inheritdoc cref="IBookBorrowRepository.GetOverdueBooksCount"/>
    public async Task<int> GetOverdueBooksCount(DateOnly date, CancellationToken cancellationToken = default)
    {
        return await _dbContext.BookBorrows
            .CountAsync(x =>
                    x.DueDate < date &&
                    (
                        x.ReturnDate == null ||
                        x.ReturnDate > date
                    ),
                cancellationToken);
    }
}