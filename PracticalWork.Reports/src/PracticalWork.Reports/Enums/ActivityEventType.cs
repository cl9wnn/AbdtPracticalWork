namespace PracticalWork.Reports.Enums;

public enum ActivityEventType
{
    /// <summary>
    /// Неизвестный
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    /// Книга создана
    /// </summary>
    BookCreated = 10,
    
    /// <summary>
    /// Книга архивирована
    /// </summary>
    BookArchived = 20,
    
    /// <summary>
    /// Книга взята читателем
    /// </summary>
    BookBorrowed = 30,
    
    /// <summary>
    /// Книга возвращена читателем
    /// </summary>
    BookReturned = 40,
    
    /// <summary>
    /// Карточка читателя закрыта
    /// </summary>
    ReaderClosed = 50,
    
    /// <summary>
    /// Карточка читателя создана
    /// </summary>
    ReaderCreated = 60
}