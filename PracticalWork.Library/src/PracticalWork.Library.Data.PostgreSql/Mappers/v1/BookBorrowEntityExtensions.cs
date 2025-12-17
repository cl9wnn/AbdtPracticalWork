using PracticalWork.Library.Data.PostgreSql.Entities;
using PracticalWork.Library.Dtos;

namespace PracticalWork.Library.Data.PostgreSql.Mappers.v1;

public static class BookBorrowEntityExtensions
{
    public static BorrowedBookDto ToBorrowedBookDto(this BookBorrowEntity bookBorrowEntity)
    {
        return new BorrowedBookDto
        {
            BookId = bookBorrowEntity.BookId,
            Title = bookBorrowEntity.Book.Title,
            Authors = bookBorrowEntity.Book.Authors,
            Description = bookBorrowEntity.Book.Description,
            Year = bookBorrowEntity.Book.Year,
            BorrowDate = bookBorrowEntity.BorrowDate,
            DueDate = bookBorrowEntity.DueDate,
        };
    }
}