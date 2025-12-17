namespace PracticalWork.Library.Dtos;

/// <summary>
/// 
/// </summary>
public class SearchBooksDto
{
    public PaginationDto Pagination { get; set; }
    public BookFilterDto BookFilter { get; set; }
}