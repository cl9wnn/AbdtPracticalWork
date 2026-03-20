using PracticalWork.Library.Abstractions.Services.Infrastructure;

namespace PracticalWork.Library.Dtos;

/// <summary>
/// Модель письма для еженедельного отчета для администрации
/// </summary>
public class WeeklyReportEmailMessageDto: IEmailTemplate
{
    /// <summary>
    /// Дата начала отчетного периода
    /// </summary>
    public DateOnly StartDate { get; init; }

    /// <summary>
    /// Дата окончания отчетного периода
    /// </summary>
    public DateOnly EndDate { get; init; }

    /// <summary>
    /// Количество новых книг
    /// </summary>
    public int NewBooksCount { get; init; }

    /// <summary>
    /// Количество новых читателей
    /// </summary>
    public int NewReadersCount { get; init; }

    /// <summary>
    /// Количество выданных книг
    /// </summary>
    public int IssuedBooksCount { get; init; }

    /// <summary>
    /// Количество возвращенных книг
    /// </summary>
    public int ReturnedBooksCount { get; init; }

    /// <summary>
    /// Количество просроченных выдач
    /// </summary>
    public int OverdueBooksCount { get; init; }

    /// <summary>
    /// Ссылка для скачивания отчета
    /// </summary>
    public string ReportDownloadLink { get; init; }

    /// <summary>
    /// Дата генерации отчета
    /// </summary>
    public DateTime GeneratedAt { get; init; }

    /// <summary>
    /// Название html файла шаблона письма
    /// </summary>
    /// <remarks>Необходимо для сопоставления модели шаблона с html шаблоном письма</remarks>
    public string TemplateName { get; } = "WeeklyReport";
}