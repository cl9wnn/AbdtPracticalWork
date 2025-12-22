namespace PracticalWork.Reports.Contracts.v1.Abstracts;

/// <summary>
/// Ответ на запрос о получении списка данных с пагинацией
/// </summary>
/// <param name="Page">Номер страницы (по умолчанию - 1)</param>
/// <param name="PageSize">Размер страницы (по умолчанию - 20)</param>
/// <param name="Items">Список элементов</param>
/// <typeparam name="TItem">Элемент списка</typeparam>
public record PagedResponse<TItem>(int Page, int PageSize, IReadOnlyList<TItem> Items);
