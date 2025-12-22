namespace PracticalWork.Reports.Dtos;

/// <summary>
/// DTO для представления страницы со списком данных для пагинации
/// </summary>
/// <typeparam name="TItem">Тип предмета из списка</typeparam>
public class PageDto<TItem>
{
    /// <summary>Номер страницы</summary>
    public int Page { get; set; }
    
    /// <summary>Размер страницы</summary>
    public int PageSize { get; set; }
    
    /// <summary>Список предметов</summary>
    public IReadOnlyList<TItem> Items { get; set; }
}