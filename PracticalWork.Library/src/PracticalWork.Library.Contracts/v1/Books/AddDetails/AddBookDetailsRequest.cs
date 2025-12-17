using Microsoft.AspNetCore.Http;

namespace PracticalWork.Library.Contracts.v1.Books.AddDetails;

/// <summary>
/// Запрос на добавления деталей по книге
/// </summary>
/// <param name="CoverImage">Обложка книги</param>
/// <param name="Description">Краткое описание книги</param>
public sealed record AddBookDetailsRequest(IFormFile CoverImage, string Description);