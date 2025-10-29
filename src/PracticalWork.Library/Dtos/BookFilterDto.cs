using PracticalWork.Library.Enums;

namespace PracticalWork.Library.Dtos;

public class BookFilterDto
{
    public BookStatus Status { get; set; }
    public BookCategory Category { get; set; }
    public string Author { get; set; }
}