namespace PracticalWork.Library.Dtos;

public class BorrowedBookDto
{
    public Guid BookId { get; set; }
    public DateOnly BorrowDate { get; set; }
    public DateOnly DueDate { get; set; }
}