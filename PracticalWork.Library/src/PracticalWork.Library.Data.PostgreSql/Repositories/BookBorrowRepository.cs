using Microsoft.EntityFrameworkCore;
using PracticalWork.Library.Abstractions.Storage;
using PracticalWork.Library.Data.PostgreSql.Entities;
using PracticalWork.Library.Data.PostgreSql.Mappers.v1;
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
        var bookBorrowEntity = await _dbContext.BookBorrows
            .Include(bookBorrowEntity => bookBorrowEntity.Reader)
            .FirstOrDefaultAsync(b => b.BookId == bookId && b.Status == BookIssueStatus.Issued, 
                cancellationToken: cancellationToken);

        if (bookBorrowEntity == null)
        {
            throw new EntityNotFoundException("Отсутствует активная запись о выдаче книги!");
        }

        return new ReaderInfoDto
        {
            Id = bookBorrowEntity.ReaderId,
            FullName = bookBorrowEntity.Reader.FullName,
            PhoneNumber = bookBorrowEntity.Reader.PhoneNumber,
            Email = bookBorrowEntity.Reader.Email
        };
    }
    
    /// <inheritdoc cref="IBookBorrowRepository.GetBorrowsDueInDays"/>
    public async Task<IReadOnlyList<BorrowedBookDto>> GetBorrowsDueInDays(int days, CancellationToken cancellationToken)
    {
        var targetDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(days);
        
        return await _dbContext.BookBorrows
            .Include(x => x.Reader)
            .Include(x => x.Book)
            .Where(x =>
                x.Status == BookIssueStatus.Issued &&
                x.DueDate == targetDate)
            .Select(x => x.ToBorrowedBookDto())
            .ToListAsync(cancellationToken: cancellationToken);
    }
}