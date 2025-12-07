using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PracticalWork.Library.Abstractions.Services.Domain;
using PracticalWork.Library.Contracts.v1.Abstracts;
using PracticalWork.Library.Contracts.v1.Books.AddDetails;
using PracticalWork.Library.Contracts.v1.Library.Borrow;
using PracticalWork.Library.Contracts.v1.Library.Get;
using PracticalWork.Library.Contracts.v1.Library.Return;
using PracticalWork.Library.Controllers.Attributes;
using PracticalWork.Library.Controllers.Mappers.v1;
using PracticalWork.Library.Models;

namespace PracticalWork.Library.Controllers.Api.v1;

/// <summary>
/// Контроллер для работы с библиотекой
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/library")]
public class LibraryController : Controller
{
    private readonly ILibraryService _libraryService;

    public LibraryController(ILibraryService libraryService)
    {
        _libraryService = libraryService;
    }

    /// <summary> Получение списка книг библиотеки</summary>
    [HttpGet("books")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(PagedResponse<GetLibraryBookResponse>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetLibraryBooks([FromQuery] GetLibraryBooksRequest request)
    {
        var result = await _libraryService.GetLibraryBooksPage(
            request.ToBookFilterDto(),
            request.ToLibraryBookPaginationDto());

        return Ok(new PagedResponse<GetLibraryBookResponse>(
            result.Page,
            result.PageSize,
            result.Items.ToLibraryBookResponseList()));
    }

    /// <summary> Выдача книги</summary>
    [HttpPost("borrow")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(BorrowBookResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> BorrowBook(BorrowBookRequest request)
    {
        var result = await _libraryService.BorrowBook(request.BookId, request.ReaderId);

        return Ok(new BorrowBookResponse(result));
    }

    /// <summary> Возврат книги</summary>
    [HttpPost("return")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> ReturnBook(ReturnBookRequest request)
    {
        await _libraryService.ReturnBook(request.BookId);

        return Ok();
    }

    /// <summary> Получение деталей книги по ее идентификатору</summary>
    [HttpGet("books/{id:guid}/details")]
    [EntityExists<IBookService, Book>]
    [Produces("application/json")]
    [ProducesResponseType(typeof(BookDetailsResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetBookDetailsById(Guid id)
    {
        var result = await _libraryService.GetBookDetailsById(id);

        return Ok(result.ToBookDetailsResponse());
    }

    /// <summary> Получение деталей книги по ее названию</summary>
    [HttpGet("books/{title}/details")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(BookDetailsResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetBookDetailsByTitle(string title)
    {
        var result = await _libraryService.GetBookDetailsByTitle(title);

        return Ok(result.ToBookDetailsResponse());
    }
}