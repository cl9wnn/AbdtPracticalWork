namespace PracticalWork.Library.Options.Email;

/// <summary>
/// Настройки шаблонов email сообщений в системе библиотеки.
/// Содержит конфигурацию для всех типов отправляемых email.
/// </summary>
public class EmailTemplateSettings
{
    /// <summary>
    /// Настройки шаблона для email напоминаний о возврате книг
    /// </summary>
    public ReturnReminderTemplate ReturnReminder { get; set; } = new();
    
    /// <summary>
    /// Настройки шаблона для email с еженедельными отчетами
    /// </summary>
    public WeeklyReportTemplate WeeklyReport { get; set; } = new();
}