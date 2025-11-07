using PracticalWork.Library.Contracts.v1.Books.Request;
using PracticalWork.Library.Contracts.v1.Books.Response;
using PracticalWork.Library.Contracts.v1.Readers.Response;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Enums;
using PracticalWork.Library.Models;

namespace PracticalWork.Library.Controllers.Mappers.v1;

public static class BooksExtensions
{
    public static Book ToBook(this CreateBookRequest request) =>
        new()
        {
            Authors = request.Authors,
            Title = request.Title,
            Description = request.Description,
            Year = request.Year,
            Category = (BookCategory)request.Category
        };

    public static Book ToBook(this UpdateBookRequest request) =>
        new()
        {
            Authors = request.Authors,
            Title = request.Title,
            Description = request.Description,
            Year = request.Year,
        };

    public static BookFilterDto ToBookFilterDto(this GetBooksRequest request) =>
        new()
        {
            Status = (BookStatus)request.BookStatus,
            Category = (BookCategory)request.BookCategory,
            Author = request.Author,
        };

    public static GetBookResponse ToBookResponse(this Book book) =>
        new(
            Title: book.Title,
            Authors: book.Authors,
            Description: book.Description,
            Year: book.Year
        );

    public static IReadOnlyList<GetBookResponse> ToBookResponseList(this IEnumerable<Book> books) =>
        books.Select(b => b.ToBookResponse()).ToList();

    public static ArchiveBookResponse ToArchiveBookResponse(this ArchiveBookDto dto) =>
        new(
            Id: dto.Id,
            Title: dto.Title,
            ArchivedAt: dto.ArchivedAt
        );

    public static BookDetailsResponse ToBookDetailsResponse(this BookDetailsDto dto) =>
        new(
            Id: dto.Id,
            Title: dto.Title,
            Category: (Contracts.v1.Enums.BookCategory)dto.Category,
            Authors: dto.Authors,
            Description: dto.Description,
            Year: dto.Year,
            CoverImagePath: dto.CoverImagePath,
            Status: (Contracts.v1.Enums.BookStatus)dto.Status,
            IsArchived: dto.IsArchived
        );

    public static GetBorrowedBookResponse ToBorrowedBookResponse(this BorrowedBookDto dto) =>
        new(
            BookId: dto.BookId,
            BorrowDate: dto.BorrowDate,
            DueDate: dto.DueDate
        );

    public static IReadOnlyList<GetBorrowedBookResponse> ToBorrowedBookResponseList(
        this IEnumerable<BorrowedBookDto> books) =>
        books.Select(b => b.ToBorrowedBookResponse()).ToList();
}