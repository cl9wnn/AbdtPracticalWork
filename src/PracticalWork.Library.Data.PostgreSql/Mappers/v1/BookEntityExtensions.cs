using PracticalWork.Library.Data.PostgreSql.Entities;
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
    
    public static BookCategory GetBookCategory(this AbstractBookEntity entity)
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