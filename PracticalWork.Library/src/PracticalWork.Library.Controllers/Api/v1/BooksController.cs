using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PracticalWork.Library.Abstractions.Services.Domain;
using PracticalWork.Library.Contracts.v1.Abstracts;
using PracticalWork.Library.Contracts.v1.Books.AddDetails;
using PracticalWork.Library.Contracts.v1.Books.Archive;
using PracticalWork.Library.Contracts.v1.Books.Create;
using PracticalWork.Library.Contracts.v1.Books.Get;
using PracticalWork.Library.Contracts.v1.Books.Update;
using PracticalWork.Library.Controllers.Attributes;
using PracticalWork.Library.Controllers.Mappers.v1;
using PracticalWork.Library.Models;

namespace PracticalWork.Library.Controllers.Api.v1;

/// <summary>
/// Контроллер для работы с книгами
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/books")]
public class BooksController : Controller
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary> Создание новой книги</summary>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(CreateBookResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateBook(CreateBookRequest request)
    {
        var result = await _bookService.CreateBook(request.ToBook());

        return Ok(new CreateBookResponse(result));
    }

    /// <summary> Редактирование книги </summary>
    [HttpPut("{id:guid}")]
    [EntityExists<IBookService, Book>]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateBook(Guid id, UpdateBookRequest request)
    {
        await _bookService.UpdateBook(id, request.ToBook());

        return Ok();
    }

    /// <summary> Перевод книги в архив</summary>
    [HttpPost("{id:guid}/archive")]
    [EntityExists<IBookService, Book>]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ArchiveBookResponse), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> ArchiveBook(Guid id)
    {
        var result = await _bookService.ArchiveBook(id);

        return Ok(result.ToArchiveBookResponse());
    }

    /// <summary> Получение списка книг</summary>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(PagedResponse<GetBookResponse>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetBooks([FromQuery] GetBooksRequest request)
    {
        var result = await _bookService.GetBooksPage(
            request.ToBookFilterDto(),
            request.ToBookPaginationDto());

        return Ok(new PagedResponse<GetBookResponse>(
            result.Page,
            result.PageSize,
            result.Items.ToBookResponseList()));
    }

    /// <summary> Добавление деталей книги</summary>
    [HttpPost("{id:guid}/details")]
    [EntityExists<IBookService, Book>]
    [ValidateImageFile(5 * 1024 * 1024)]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> AddBookDetails(Guid id, [FromForm] AddBookDetailsRequest request)
    {
        await using var imageStream = request.CoverImage.OpenReadStream();
        var fileExtension = Path.GetExtension(request.CoverImage.FileName);
        await _bookService.AddBookDetails(id, request.Description, imageStream, fileExtension);

        return Ok();
    }
}