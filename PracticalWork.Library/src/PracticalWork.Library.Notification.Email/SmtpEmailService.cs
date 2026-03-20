using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.Extensions.Logging;
using PracticalWork.Library.Abstractions.Services.Infrastructure;

namespace PracticalWork.Library.Notification.Email;

/// <summary>
/// Сервис для отправки email
/// </summary>
public class SmtpEmailService: IEmailService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IFluentEmail fluentEmail, ILogger<SmtpEmailService> logger)
    {
        _fluentEmail = fluentEmail;
        _logger = logger;
    }
    
    /// <inheritdoc cref="IEmailService.SendAsync{TTemplate}"/>
    public async Task SendAsync<TTemplate>(string to, string subject,
        TTemplate message, CancellationToken cancellationToken = default) where TTemplate : IEmailTemplate
    {
        try
        {
            var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", message.TemplateName + ".cshtml");

            var result = await _fluentEmail
                .To(to)
                .Subject(subject)
                .UsingTemplateFromFile(templatePath, message)
                .SendAsync(cancellationToken);

            if (result.Successful)
            {
                _logger.LogInformation("Письмо на ящик {to} успешно отправлено", to);
            }
            else
            {
                _logger.LogError("Возникла ошибка при отправке письма на адрес {to}. Ошибка: {errors}", to,
                    string.Join(",\n ", result.ErrorMessages));
                throw new Exception(string.Join(",\n ", result.ErrorMessages));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка сервиса отправки электронных писем");
            throw;
        }
    }
    
    /// <inheritdoc cref="IEmailService.SendBulkAsync{TTemplate}"/>
    public async Task SendBulkAsync<TTemplate>(IEnumerable<string> to, string subject,
        TTemplate message, CancellationToken cancellationToken = default) where TTemplate : IEmailTemplate
    {
        try
        {
            var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", message.TemplateName + ".cshtml");

            var result = await _fluentEmail
                .To(to.Select(x => new Address(x)))
                .Subject(subject)
                .UsingTemplateFromFile(templatePath, message)
                .SendAsync(cancellationToken);

            if (result.Successful)
            {
                _logger.LogInformation("Рассылка писем на ящики успешно совершена");
            }
            else
            {
                _logger.LogError("Возникла ошибка при рассылке писем. Ошибка: {errors}",
                    string.Join(",\n ", result.ErrorMessages));
                throw new Exception(string.Join(",\n ", result.ErrorMessages));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка сервиса отправки электронных писем");
            throw;
        }
    }
}