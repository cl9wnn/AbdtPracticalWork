using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PracticalWork.Reports.Abstractions.Services.Infrastructure;

namespace PracticalWork.Reports.Notification.Email;

public static class Entry
{
    /// <summary>
    /// Регистрация зависимостей для почтового сервиса
    /// </summary>
    public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        var emailSettings = configuration.GetSection("App:EmailSettings").Get<EmailSettings>();
        
        services.AddScoped<IEmailService, SmtpEmailService>();
        
        services
            .AddFluentEmail(emailSettings!.SenderEmail)
            .AddRazorRenderer()
            .AddSmtpSender(() => new SmtpClient()
            {
                Host = emailSettings.SmtpServer,
                Port = emailSettings.SmtpPort,
                EnableSsl = emailSettings.UseSsl,
                Credentials = new System.Net.NetworkCredential(
                    emailSettings.UserName,
                    emailSettings.Password
                )
            });
        
        return services;
    }
}