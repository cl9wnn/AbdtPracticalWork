namespace PracticalWork.Library.Dtos;

public class LibraryBookDto
{
    public string Title { get; set; }
    public IReadOnlyList<string> Authors { get; set; }
    public string Description { get; set; }
    public int Year { get; set; }
    public Guid? ReaderId { get; set; }
    public DateOnly? BorrowDate { get; set; }
    public DateOnly? DueDate { get; set; }
}