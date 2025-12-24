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
/// Репозиторий для работы с карточками читателей
/// </summary>
public class ReaderRepository: IReaderRepository
{
    private readonly AppDbContext _appDbContext;

    public ReaderRepository(AppDbContext dbContext)
    {
        _appDbContext = dbContext;
    }
    
    /// <inheritdoc cref="IEntityRepository{Guid,Reader}.GetById"/>
    public async Task<Reader> GetById(Guid id)
    {
        var readerEntity = await _appDbContext.Readers
            .FindAsync(id);

        if (readerEntity == null)
        {
            throw new EntityNotFoundException("Карточка читателя не найдена по данному идентификатору!");
        }

        return readerEntity.ToReader();
    }

    /// <inheritdoc cref="IReaderRepository.GetBorrowedBooks"/>
    public async Task<IReadOnlyList<BorrowedBookDto>> GetBorrowedBooks(Guid readerId)
    {
        var borrowedBooks = await _appDbContext.BookBorrows
            .Include(b => b.Book)
            .Where(b => b.ReaderId == readerId && b.Status == BookIssueStatus.Issued)
            .AsNoTracking()
            .Select(b => b.ToBorrowedBookDto())
            .ToListAsync();

        return borrowedBooks;
    }
    
    /// <inheritdoc cref="IEntityRepository{Guid,Reader}.GetAll"/>
    public async Task<ICollection<Reader>> GetAll()
    {
        return await _appDbContext.Readers
            .Select(r => r.ToReader())
            .AsNoTracking()
            .ToListAsync();
    }
    
    /// <inheritdoc cref="IEntityRepository{Guid,Reader}.Add"/>
    public async Task<Guid> Add(Reader reader)
    {
        var readerEntity = new ReaderEntity
        {
            CreatedAt = DateTime.UtcNow,
            FullName = reader.FullName,
            PhoneNumber = reader.PhoneNumber,
            ExpiryDate = reader.ExpiryDate,
            IsActive = reader.IsActive,
        };
        
         _appDbContext.Readers.Add(readerEntity);
        await _appDbContext.SaveChangesAsync();
        
        return readerEntity.Id;
    }

    /// <inheritdoc cref="IEntityRepository{Guid,Reader}.Update"/>
    public async Task<Reader> Update(Guid id, Reader reader)
    {
        var readerEntity = await _appDbContext.Readers
            .FindAsync(id);

        if (readerEntity == null)
        {
            throw new EntityNotFoundException("Карточка читателя не найдена по данному идентификатору");
        }
        
        readerEntity.ExpiryDate = reader.ExpiryDate;
        readerEntity.IsActive = reader.IsActive;
        
        await _appDbContext.SaveChangesAsync();
        
        return readerEntity.ToReader();
    }

    /// <inheritdoc cref="IEntityRepository{Guid,Reader}.Delete"/>
    public async Task Delete(Guid id)
    {
        var readerEntity = await _appDbContext.Readers
            .FindAsync(id);

        if (readerEntity == null)
        {
            throw new EntityNotFoundException("Карточка читателя не найдена по данному идентификатору");
        }
        
        _appDbContext.Readers.Remove(readerEntity);
        await _appDbContext.SaveChangesAsync();
    }
    
    /// <inheritdoc cref="IEntityRepository{Guid,Reader}.Exists"/>
    public async Task<bool> Exists(Guid id)
    {
        return await _appDbContext.Readers.AnyAsync(b => b.Id == id);
    }

    /// <inheritdoc cref="IReaderRepository.Exists(string)"/>
    public async Task<bool> Exists(string phoneNumber)
    {
        return await _appDbContext.Readers.AnyAsync(b => b.PhoneNumber == phoneNumber);
    }
}