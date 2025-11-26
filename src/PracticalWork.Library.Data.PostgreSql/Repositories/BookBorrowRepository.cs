using Microsoft.EntityFrameworkCore;
using PracticalWork.Library.Abstractions.Storage;
using PracticalWork.Library.Data.PostgreSql.Entities;
using PracticalWork.Library.Enums;
using PracticalWork.Library.Exceptions;
using PracticalWork.Library.Models;

namespace PracticalWork.Library.Data.PostgreSql.Repositories;

public class BookBorrowRepository: IBookBorrowRepository
{
    private readonly AppDbContext _dbContext;

    public BookBorrowRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    /// <inheritdoc cref="IBookBorrowRepository.Create"/>
    public async Task<Guid> Create(Guid bookId, Guid readerId, Borrow bookBorrow)
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
            .AddAsync(bookBorrowEntity);
        await _dbContext.SaveChangesAsync();
        
        return bookBorrowEntity.Id;
    }

    /// <inheritdoc cref="IBookBorrowRepository.GetActiveBorrowByBookId"/>
    public async Task<Borrow> GetActiveBorrowByBookId(Guid bookId)
    {
        var bookBorrowEntity = await _dbContext.BookBorrows
            .FirstOrDefaultAsync(b => b.BookId == bookId && b.Status == BookIssueStatus.Issued);

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
    public async Task Update(Guid bookId, Borrow bookBorrow)
    {
        var bookBorrowEntity = await _dbContext.BookBorrows
            .FirstOrDefaultAsync(b => b.BookId == bookId && b.Status == BookIssueStatus.Issued);
        
        if (bookBorrow == null)
        {
            throw new EntityNotFoundException("Отсутствует активная запись о выдаче книги!");
        }
        
        bookBorrowEntity.ReturnDate = bookBorrow.ReturnDate;
        bookBorrowEntity.Status = bookBorrow.Status;
        bookBorrowEntity.UpdatedAt = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync();
    }
}