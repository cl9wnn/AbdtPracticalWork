namespace PracticalWork.Reports.Abstractions.Services.Infrastructure;

/// <summary>
/// Контракт сервиса для отправки email
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Отправка сообщения на почту
    /// </summary>
    /// <param name="to">Почта получателя письма</param>
    /// <param name="subject">Тема письма</param>
    /// <param name="message">Модель сообщения</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <typeparam name="TTemplate">Тип модели сообщения (шаблон письма)</typeparam>
    Task SendAsync<TTemplate>(string to, string subject, TTemplate message,
        CancellationToken cancellationToken = default) where TTemplate : IEmailTemplate;

    /// <summary>
    /// Массовая рассылка сообщений на почты
    /// </summary>
    /// <param name="to">Список почт получателей писем</param>
    /// <param name="subject">Тема письма</param>
    /// <param name="message">Модель сообщения</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <typeparam name="TTemplate">Тип модели сообщения (шаблон письма)</typeparam>
    Task SendBulkAsync<TTemplate>(IEnumerable<string> to, string subject,
        TTemplate message, CancellationToken cancellationToken = default) where TTemplate : IEmailTemplate;
}