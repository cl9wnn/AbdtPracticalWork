namespace PracticalWork.Library.Contracts.v1.Books.Response;

/// <summary>
/// 
/// </summary>
/// <param name="Page">Номер страницы</param>
/// <param name="PageSize">Размер страницы</param>
/// <param name="Items">Список элементов</param>
/// <typeparam name="TItem">Элемент списка</typeparam>
public record PagedResponse<TItem>(int Page, int PageSize, IReadOnlyList<TItem> Items);
