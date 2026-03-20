using PracticalWork.Library.Abstractions.Services.Infrastructure;

namespace PracticalWork.Library.Dtos;

/// <summary>
/// Модель письма для автоматического напоминания о возврате книг
/// </summary>
public class ReturnReminderEmailMessageDto: IEmailTemplate
{
    /// <summary>
    /// ФИО читателя
    /// </summary>
    public string ReaderFullName { get; init; }

    /// <summary>
    /// Название книги
    /// </summary>
    public string BookTitle { get; init; }

    /// <summary>
    /// Авторы книги
    /// </summary>
    public string BookAuthors { get; init; }

    /// <summary>
    /// Дата возврата в формате "дд.мм.гггг"
    /// </summary>
    public DateOnly ReturnDate { get; init; }
    
    /// <summary>
    /// Количество дней до возврата
    /// </summary>
    public int DaysLeft { get; init; }

    /// <summary>
    /// Физический адрес библиотеки для отображения в email сообщениях
    /// </summary>
    public string LibraryAddress { get; init; }

    /// <summary>
    /// Контактный телефон библиотеки для отображения в email сообщениях
    /// </summary>
    public string LibraryPhone { get; init; }

    /// <summary>
    /// Часы работы библиотеки для отображения в email сообщениях
    /// </summary>
    public string WorkingHours { get; init; }

    /// <summary>
    /// Название html файла шаблона письма
    /// </summary>
    /// <remarks>Необходимо для сопоставления модели шаблона с html шаблоном письма</remarks>
    public string TemplateName { get; } = "ReturnReminderTemplate";
}