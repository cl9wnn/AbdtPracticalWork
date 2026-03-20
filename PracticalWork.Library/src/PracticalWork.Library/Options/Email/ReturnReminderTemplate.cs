namespace PracticalWork.Library.Options.Email;

/// <summary>
/// Шаблон для email напоминаний о возврате книг
/// </summary>
public class ReturnReminderTemplate
{
    /// <summary>
    /// Шаблон темы (subject) email напоминания
    /// </summary>
    public string SubjectTemplate { get; set; } 
    
    /// <summary>
    /// Количество дней до срока возврата, за которое отправляется напоминание
    /// </summary>
    public int DaysBeforeDueDate { get; set; } 
    
    /// <summary>
    /// Название библиотеки для отображения в email сообщениях
    /// </summary>
    public string LibraryName { get; set; } 
    
    /// <summary>
    /// Физический адрес библиотеки для отображения в email сообщениях
    /// </summary>
    public string LibraryAddress { get; set; }
    
    /// <summary>
    /// Контактный телефон библиотеки для отображения в email сообщениях
    /// </summary>
    public string LibraryPhone { get; set; } 
    
    /// <summary>
    /// Часы работы библиотеки для отображения в email сообщениях
    /// </summary>
    public string WorkingHours { get; set; }
}