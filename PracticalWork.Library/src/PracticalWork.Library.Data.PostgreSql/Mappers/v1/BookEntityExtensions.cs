using JetBrains.Annotations;
using PracticalWork.Library.Data.PostgreSql.Entities;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Enums;
using PracticalWork.Library.Models;

namespace PracticalWork.Library.Data.PostgreSql.Mappers.v1;

public static class BookEntityExtensions
{
    public static Book ToBook(this AbstractBookEntity bookEntity)
    {
        return new Book
        {
            Title = bookEntity.Title,
            Authors = bookEntity.Authors,
            Description = bookEntity.Description,
            Year = bookEntity.Year,
            Category = bookEntity.GetBookCategory(),
            Status = bookEntity.Status,
            CoverImagePath = bookEntity.CoverImagePath,
            IsArchived = bookEntity.Status == BookStatus.Archived,
        };
    }

    public static BookListDto ToBookListDto(this AbstractBookEntity bookEntity)
    {
        return new BookListDto
        {
            Title = bookEntity.Title,
            Authors = bookEntity.Authors,
            Description = bookEntity.Description,
            Year = bookEntity.Year,
        };
    }
    
    public static LibraryBookDto ToLibraryBookDto(this AbstractBookEntity bookEntity, BookBorrowEntity activeBorrow)
    {
        return new LibraryBookDto
        {
            Title = bookEntity.Title,
            Authors = bookEntity.Authors,
            Description = bookEntity.Description,
            Year = bookEntity.Year,
            ReaderId = activeBorrow?.ReaderId,
            BorrowDate = activeBorrow?.BorrowDate,
            DueDate = activeBorrow?.DueDate,
        };
    }
    
    public static BookDetailsDto ToBookDetailsDto(this AbstractBookEntity bookEntity)
    {
        return new BookDetailsDto
        {
            Id = bookEntity.Id,
            Title = bookEntity.Title,
            Authors = bookEntity.Authors,
            Description = bookEntity.Description,
            Year = bookEntity.Year,
            Category = bookEntity.GetBookCategory(),
            Status = bookEntity.Status,
            CoverImagePath = bookEntity.CoverImagePath,
            IsArchived = bookEntity.Status == BookStatus.Archived,
        };
    }
    
    private static BookCategory GetBookCategory(this AbstractBookEntity entity)
    {
        return entity switch
        {
            ScientificBookEntity => BookCategory.ScientificBook,
            EducationalBookEntity => BookCategory.EducationalBook,
            FictionBookEntity => BookCategory.FictionBook,
            _ => throw new ArgumentOutOfRangeException($"Неизвестный тип книги: {entity.GetType().Name}")
        };
    }
}