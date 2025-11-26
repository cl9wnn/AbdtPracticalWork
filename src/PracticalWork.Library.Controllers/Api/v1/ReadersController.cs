using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PracticalWork.Library.Abstractions.Services.Domain;
using PracticalWork.Library.Contracts.v1.Readers.Request;
using PracticalWork.Library.Contracts.v1.Readers.Response;
using PracticalWork.Library.Controllers.Attributes;
using PracticalWork.Library.Controllers.Mappers.v1;
using PracticalWork.Library.Models;

namespace PracticalWork.Library.Controllers.Api.v1;

/// <summary>
/// Контроллер для работы с карточками читателей
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/readers")]
public class ReadersController: Controller
{
    private readonly IReaderService _readerService;

    public ReadersController(IReaderService readerService)
    {
        _readerService = readerService;
    }

    /// <summary> Создание новой карточки читателя</summary>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(CreateReaderResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateReader(CreateReaderRequest request)
    {
        var result = await _readerService.CreateReader(request.ToReader());
        
        return Ok(new CreateReaderResponse(result));
    }
    
    /// <summary> Продление срока действия карточки читателя</summary>
    [HttpPost("{id:guid}/extend")]
    [EntityExists<IReaderService, Reader>]
    [ProducesResponseType(200)]   
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> ExtendReader(Guid id, ExtendReaderRequest request)
    {
        await _readerService.ExtendReader(id, request.NewExpiryDate);
        
        return Ok();
    }

    /// <summary> Закрытие карточки читателя</summary>
    [HttpPost("{id:guid}/close")]
    [EntityExists<IReaderService, Reader>]
    [ProducesResponseType(200)]  
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CloseReader(Guid id)
    {
        await _readerService.CloseReader(id);
        
        return Ok();
    }

    /// <summary> Получение взятых книг</summary>
    [HttpGet("{id:guid}/books")]
    [EntityExists<IReaderService, Reader>]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IReadOnlyList<GetBorrowedBookResponse>), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetBorrowedBooks(Guid id)
    {
        var result = await _readerService.GetBorrowedBooks(id);
        
        return Ok(result.ToBorrowedBookResponseList());
    }
}