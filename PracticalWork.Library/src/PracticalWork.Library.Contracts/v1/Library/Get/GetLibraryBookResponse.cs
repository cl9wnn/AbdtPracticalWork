using PracticalWork.Library.Contracts.v1.Abstracts;

namespace PracticalWork.Library.Contracts.v1.Library.Get;

/// <summary>
/// Ответ на запрос получения книг библиотеки
/// </summary>
/// <param name="Title">Название книги</param>
/// <param name="Authors">Авторы</param>
/// <param name="Description">Краткое описание книги</param>
/// <param name="Year">Год издания</param>
/// <param name="ReaderId">Идентификатор карточки читателя, которому выдана книга</param>
/// <param name="BorrowDate">Дата выдачи книги </param>
/// <param name="DueDate">Срок возврата книги</param>
public sealed record GetLibraryBookResponse(
    string Title,
    IReadOnlyList<string> Authors,
    string Description,
    int Year,
    Guid? ReaderId,
    DateOnly? BorrowDate,
    DateOnly? DueDate)
    : AbstractBook(Title, Authors, Description, Year);