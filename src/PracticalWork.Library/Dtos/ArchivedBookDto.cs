namespace PracticalWork.Library.Dtos;

/// <summary>
/// DTO для представления архивированной книги
/// </summary>
public class ArchivedBookDto
{
   /// <summary>Идентификатор книги</summary>
   public Guid Id { get; set; }
   
   /// <summary>Название книги</summary>
   public string Title { get; set; }
   
   /// <summary>Дата перевода в архив</summary>
   public DateTime ArchivedAt { get; set; }
}