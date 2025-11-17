namespace PracticalWork.Library.Dtos;

public class BookListDto
{
    public string Title { get; set; } 
    public IReadOnlyList<string> Authors { get; set; }
    public string Description { get; set; }
    public int Year { get; set; }
}