using PracticalWork.Library.Contracts.v1.Books.Request;
using PracticalWork.Library.Contracts.v1.Books.Response;
using PracticalWork.Library.Contracts.v1.Library.Request;
using PracticalWork.Library.Contracts.v1.Library.Response;
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
    
    public static PaginationDto ToBookPaginationDto(this GetBooksRequest request) =>
        new()
        {
            Page = request.Page > 0 ?  request.Page : 1,
            PageSize = request.PageSize  is > 0 and <= 100 ? request.PageSize : 10
        };
    
    public static PaginationDto ToLibraryBookPaginationDto(this GetLibraryBooksRequest request) =>
        new()
        {
            Page = request.Page > 0 ?  request.Page : 1,
            PageSize = request.PageSize  is > 0 and <= 100 ? request.PageSize : 10
        };

    public static BookFilterDto ToBookFilterDto(this GetLibraryBooksRequest request) =>
        new()
        {
            AvailableOnly = request.AvailableOnly,
            Category = (BookCategory)request.BookCategory,
            Author = request.Author,
        };

    public static GetBookResponse ToBookResponse(this BookListDto book) =>
        new(
            Title: book.Title,
            Authors: book.Authors,
            Description: book.Description,
            Year: book.Year
        );

    public static GetLibraryBookResponse ToLibraryBookResponse(this LibraryBookDto book) =>
        new(
            Title: book.Title,
            Authors: book.Authors,
            Description: book.Description,
            Year: book.Year,
            ReaderId: book.ReaderId,
            BorrowDate:book.BorrowDate,
            DueDate: book.DueDate
        );

    public static IReadOnlyList<GetBookResponse> ToBookResponseList(this IEnumerable<BookListDto> books) =>
        books.Select(b => b.ToBookResponse()).ToList();

    public static IReadOnlyList<GetLibraryBookResponse> ToLibraryBookResponseList(
        this IEnumerable<LibraryBookDto> books) =>
        books.Select(b => b.ToLibraryBookResponse()).ToList();

    public static ArchiveBookResponse ToArchiveBookResponse(this ArchivedBookDto book) =>
        new(
            Id: book.Id,
            Title: book.Title,
            ArchivedAt: book.ArchivedAt
        );

    public static BookDetailsResponse ToBookDetailsResponse(this BookDetailsDto book) =>
        new(
            Id: book.Id,
            Title: book.Title,
            Category: (Contracts.v1.Enums.BookCategory)book.Category,
            Authors: book.Authors,
            Description: book.Description,
            Year: book.Year,
            CoverImagePath: book.CoverImagePath,
            Status: (Contracts.v1.Enums.BookStatus)book.Status,
            IsArchived: book.IsArchived
        );

    public static GetBorrowedBookResponse ToBorrowedBookResponse(this BorrowedBookDto book) =>
        new(
            BookId: book.BookId,
            Title: book.Title,
            Authors: book.Authors,
            Description: book.Description,
            Year: book.Year,    
            BorrowDate: book.BorrowDate,
            DueDate: book.DueDate
        );

    public static IReadOnlyList<GetBorrowedBookResponse> ToBorrowedBookResponseList(
        this IEnumerable<BorrowedBookDto> books) =>
        books.Select(b => b.ToBorrowedBookResponse()).ToList();
}