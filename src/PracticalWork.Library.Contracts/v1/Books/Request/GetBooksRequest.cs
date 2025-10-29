using PracticalWork.Library.Contracts.v1.Enums;

namespace PracticalWork.Library.Contracts.v1.Books.Request;

/// <summary>
/// 
/// </summary>
/// <param name="BookStatus">Статус книги</param>
/// <param name="BookCategory">Категория книги</param>
/// <param name="Author">Автор</param>
/// <param name="Page">Номер страницы</param>
/// <param name="PageSize">Размер страницы</param>
public sealed record GetBooksRequest(
    BookStatus BookStatus,
    BookCategory BookCategory,
    string Author,
    int Page,
    int PageSize);