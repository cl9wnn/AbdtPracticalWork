using PracticalWork.Library.Enums;

namespace PracticalWork.Library.Dtos;

public class BookFilterDto
{
    public BookCategory Category { get; set; }
    public string Author { get; set; }
    public BookStatus? Status { get; set; }
    public bool? AvailableOnly { get; set; }
}