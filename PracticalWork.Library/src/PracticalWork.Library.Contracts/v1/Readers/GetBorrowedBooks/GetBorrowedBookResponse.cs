using PracticalWork.Library.Contracts.v1.Abstracts;

namespace PracticalWork.Library.Contracts.v1.Readers.GetBorrowedBooks;

/// <summary>
/// Ответ на запрос о получении взятой книги
/// </summary>
/// <param name="BookId">Идентификатор книги</param>
/// <param name="Title">Название книги</param>
/// <param name="Authors">Авторы</param>
/// <param name="Description">Краткое описание книги</param>
/// <param name="Year">Год издания</param>
/// <param name="BorrowDate">Дата выдачи книги</param>
/// <param name="DueDate">Срок возврата книги</param>
public sealed record GetBorrowedBookResponse(
    Guid BookId, 
    string Title,
    IReadOnlyList<string> Authors,
    string Description,
    int Year,
    DateOnly BorrowDate,
    DateOnly DueDate)
    :AbstractBook(Title, Authors, Description, Year);