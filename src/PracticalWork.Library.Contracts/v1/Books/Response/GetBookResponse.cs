using PracticalWork.Library.Contracts.v1.Abstracts;

namespace PracticalWork.Library.Contracts.v1.Books.Response;

/// <summary>
/// Ответ на запрос получения книги
/// </summary>
/// <param name="Title">Название книги</param>
/// <param name="Authors">Авторы</param>
/// <param name="Description">Краткое описание книги</param>
/// <param name="Year">Год издания</param>
public sealed record GetBookResponse(string Title, IReadOnlyList<string> Authors, string Description, int Year)
    : AbstractBook(Title, Authors, Description, Year);