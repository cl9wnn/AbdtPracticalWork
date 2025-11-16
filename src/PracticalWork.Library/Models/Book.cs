using PracticalWork.Library.Enums;

namespace PracticalWork.Library.Models;

/// <summary>
/// Книга
/// </summary>
public sealed class Book
{
    /// <summary>Название книги</summary>
    public string Title { get; set; }

    /// <summary>Авторы</summary>
    public IReadOnlyList<string> Authors { get; set; }

    /// <summary>Краткое описание книги</summary>
    public string Description { get; set; }

    /// <summary>Год издания</summary>
    public int Year { get; set; }

    /// <summary>Категория</summary>
    public BookCategory Category { get; set; }

    /// <summary>Статус</summary>
    public BookStatus Status { get; set; }

    /// <summary>Путь к изображению обложки</summary>
    public string CoverImagePath { get; set; }

    /// <summary>В архиве</summary>
    public bool IsArchived { get; set; }

    /// <summary>Проверка перевода в архив</summary>
    public bool CanBeArchived() => Status != BookStatus.Borrow;

    /// <summary>Проверка выдачи на руки</summary>
    public bool CanBeBorrowed() => !IsArchived && Status == BookStatus.Available;

    /// <summary>Перевод в архив</summary>
    public void Archive()
    {
        if (Status == BookStatus.Archived && IsArchived)
        {
            throw new InvalidOperationException("Книга уже в архиве.");
        }
        
        if (!CanBeArchived())
            throw new InvalidOperationException("Книга не может быть заархивирована.");

        IsArchived = true;
        Status = BookStatus.Archived;
    }

    /// <summary>
    /// Обновление деталей
    /// </summary>
    /// <param name="description"> Краткое описание книги </param>
    /// <param name="coverImagePath"> Путь к изображению обложки </param>
    public void UpdateDetails(string description, string coverImagePath)
    {
        CoverImagePath = coverImagePath;

        if (description != null)
        {
            Description = description;
        }
    }

    /// <summary>
    /// Редактирование книги
    /// </summary>
    /// <param name="title">Название книги</param>
    /// <param name="authors">Авторы</param>
    /// <param name="description">Краткое описание книги</param>
    /// <param name="year">Год издания</param>
    public void ChangeInformation(string title, IReadOnlyList<string> authors, string description, int year)
    {
        Title = title;
        Authors = authors;
        Description = description;
        Year = year;
    }
}