namespace PracticalWork.Reports.Options.Email;

/// <summary>
/// Шаблон для email с еженедельными отчетами библиотеки
/// </summary>
public class WeeklyReportTemplate
{
    /// <summary>
    /// Шаблон темы (subject) email с еженедельным отчетом
    /// </summary>
    public string SubjectTemplate { get; set; }
    
    /// <summary>
    /// Массив email адресов администраторов для получения еженедельных отчетов
    /// </summary>
    public string[] AdminEmails { get; set; } 
    
    /// <summary>
    /// Количество дней хранения сгенерированных отчетов в системе
    /// </summary>
    public int ReportRetentionDays { get; set; }
}