using PracticalWork.Library.Contracts.v1.Abstracts;

namespace PracticalWork.Library.Contracts.v1.Books.Update;

/// <summary>
/// Запрос на обновление книги
/// </summary>
/// <param name="Title">Название книги</param>
/// <param name="Authors">Авторы</param>
/// <param name="Description">Краткое описание книги</param>
/// <param name="Year">Год издания</param>
public sealed record UpdateBookRequest(string Title, IReadOnlyList<string> Authors, string Description, int Year)
    : AbstractBook(Title, Authors, Description, Year);