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
/// Репозиторий для работы с карточками читателей
/// </summary>
public class ReaderRepository: IReaderRepository
{
    private readonly AppDbContext _appDbContext;

    public ReaderRepository(AppDbContext dbContext)
    {
        _appDbContext = dbContext;
    }
    
    /// <inheritdoc cref="IEntityRepository{TKey,TDto}.GetById"/>
    public async Task<Reader> GetById(Guid id, CancellationToken cancellationToken)
    {
        var readerEntity = await _appDbContext.Readers
            .FindAsync([id], cancellationToken);

        if (readerEntity == null)
        {
            throw new EntityNotFoundException("Карточка читателя не найдена по данному идентификатору!");
        }

        return readerEntity.ToReader();
    }

    /// <inheritdoc cref="IReaderRepository.GetBorrowedBooks"/>
    public async Task<IReadOnlyList<BorrowedBookDto>> GetBorrowedBooks(Guid readerId, CancellationToken cancellationToken)
    {
        var borrowedBooks = await _appDbContext.BookBorrows
            .Where(b => b.ReaderId == readerId && b.Status == BookIssueStatus.Issued)
            .AsNoTracking()
            .Select(b => new BorrowedBookDto
            {
                BookId = b.BookId,
                Title = b.Book.Title,
                Authors = b.Book.Authors,
                Description = b.Book.Description,
                Year = b.Book.Year,
                BorrowDate = b.BorrowDate,
                DueDate = b.DueDate
            })
            .ToListAsync(cancellationToken);

        return borrowedBooks;
    }

    public async Task<int> GetNewReadersCount(DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.Readers
            .CountAsync(x =>
                    x.CreatedAt >= from &&
                    x.CreatedAt <= to,
                cancellationToken);
    }

    /// <inheritdoc cref="IEntityRepository{Guid,Reader}.GetAll"/>
    public async Task<ICollection<Reader>> GetAll(CancellationToken cancellationToken)
    {
        return await _appDbContext.Readers
            .Select(r => r.ToReader())
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
    }
    
    /// <inheritdoc cref="IEntityRepository{Guid,Reader}.Add"/>
    public async Task<Guid> Add(Reader reader, CancellationToken cancellationToken)
    {
        var readerEntity = new ReaderEntity
        {
            CreatedAt = DateTime.UtcNow,
            FullName = reader.FullName,
            PhoneNumber = reader.PhoneNumber,
            Email = reader.Email,
            ExpiryDate = reader.ExpiryDate,
            IsActive = reader.IsActive,
        };
        
         _appDbContext.Readers.Add(readerEntity);
        await _appDbContext.SaveChangesAsync(cancellationToken);
        
        return readerEntity.Id;
    }

    /// <inheritdoc cref="IEntityRepository{Guid,Reader}.Update"/>
    public async Task<Reader> Update(Guid id, Reader reader, CancellationToken cancellationToken)
    {
        var readerEntity = await _appDbContext.Readers
            .FindAsync([id], cancellationToken);

        if (readerEntity == null)
        {
            throw new EntityNotFoundException("Карточка читателя не найдена по данному идентификатору");
        }
        
        readerEntity.ExpiryDate = reader.ExpiryDate;
        readerEntity.IsActive = reader.IsActive;
        
        await _appDbContext.SaveChangesAsync(cancellationToken);
        
        return readerEntity.ToReader();
    }

    /// <inheritdoc cref="IEntityRepository{Guid,Reader}.Delete"/>
    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        var readerEntity = await _appDbContext.Readers
            .FindAsync([id], cancellationToken);

        if (readerEntity == null)
        {
            throw new EntityNotFoundException("Карточка читателя не найдена по данному идентификатору");
        }
        
        _appDbContext.Readers.Remove(readerEntity);
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }
    
    /// <inheritdoc cref="IEntityRepository{Guid,Reader}.Exists"/>
    public async Task<bool> Exists(Guid id, CancellationToken cancellationToken)
    {
        return await _appDbContext.Readers.AnyAsync(b => b.Id == id, cancellationToken: cancellationToken);
    }

    /// <inheritdoc cref="IReaderRepository.Exists(string, CancellationToken)"/>
    public async Task<bool> Exists(string phoneNumber, CancellationToken cancellationToken)
    {
        return await _appDbContext.Readers.AnyAsync(b => b.PhoneNumber == phoneNumber, 
            cancellationToken: cancellationToken);
    }
}