namespace PracticalWork.Reports.Options.Email;

/// <summary>
/// Настройки шаблонов email сообщений в системе библиотеки.
/// Содержит конфигурацию для всех типов отправляемых email.
/// </summary>
public class EmailTemplateSettings
{
    /// <summary>
    /// Настройки шаблона для email с еженедельными отчетами
    /// </summary>
    public WeeklyReportTemplate WeeklyReport { get; set; } = new();
}