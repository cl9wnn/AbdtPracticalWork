namespace PracticalWork.Reports.Abstractions.Services.Infrastructure;

/// <summary>
/// Контракт для шаблона письма
/// </summary>
public interface IEmailTemplate
{
    /// <summary>
    /// Название html файла шаблона письма
    /// </summary>
    /// <remarks>Необходимо для сопоставления модели шаблона с html шаблоном письма</remarks>
    string TemplateName { get; }
}