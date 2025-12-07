using PracticalWork.Library.Contracts.v1.Enums;

namespace PracticalWork.Library.Contracts.v1.Books.Get;

/// <summary>
/// Запрос на получение списка книг
/// </summary>
/// <param name="BookStatus">Статус книги</param>
/// <param name="BookCategory">Категория книги</param>
/// <param name="Author">Автор</param>
/// <param name="Page">Номер страницы (по умолчанию - 1)</param>
/// <param name="PageSize">Размер страницы (по умолчанию - 10)</param>
public sealed record GetBooksRequest(
    BookStatus BookStatus,
    BookCategory BookCategory,
    string Author,
    int Page,
    int PageSize);