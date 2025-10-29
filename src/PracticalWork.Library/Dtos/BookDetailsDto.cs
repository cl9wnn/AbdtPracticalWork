using PracticalWork.Library.Enums;

namespace PracticalWork.Library.Dtos;

public class BookDetailsDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public BookCategory Category { get; set; }
    public IReadOnlyList<string> Authors { get; set; }
    public string Description { get; set; }
    public int Year { get; set; }
    public string CoverImagePath { get; set; }
    public BookStatus Status { get; set; }
    public bool IsArchived {get; set;}
}