using PracticalWork.Library.Contracts.v1.Enums;

namespace PracticalWork.Library.Contracts.v1.Library.Request;

/// <summary>
/// Запрос на получение книг библиотеки
/// </summary>
/// <param name="BookCategory">Категория книг</param>
/// <param name="Author">Автор</param>
/// <param name="AvailableOnly">Только доступные книги</param>
/// <param name="Page">Номер страницы (по умолчанию - 1)</param>
/// <param name="PageSize">Размер страницы (по умолчанию - 10)</param>
public sealed record GetLibraryBooksRequest(
    BookCategory BookCategory,
    string Author,
    bool AvailableOnly,
    int Page,
    int PageSize);