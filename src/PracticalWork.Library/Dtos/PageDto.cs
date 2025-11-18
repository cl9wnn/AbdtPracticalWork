namespace PracticalWork.Library.Dtos;

public class PageDto<TItem>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public IReadOnlyList<TItem> Items { get; set; }
}