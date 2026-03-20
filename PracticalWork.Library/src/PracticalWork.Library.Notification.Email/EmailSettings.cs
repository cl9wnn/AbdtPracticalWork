namespace PracticalWork.Library.Notification.Email;

/// <summary>
/// Настройки SMTP сервера для отправки email уведомлений в системе библиотеки
/// </summary>
public class EmailSettings
{
    /// <summary>
    /// Адрес SMTP сервера для отправки email
    /// </summary>
    public string SmtpServer { get; set; } = string.Empty;

    /// <summary>
    /// Порт SMTP сервера для подключения
    /// </summary>
    public int SmtpPort { get; set; }
    
    /// <summary>
    /// Определяет, используется ли SSL/TLS шифрование для подключения к SMTP серверу
    /// </summary>
    public bool UseSsl { get; set; }
    
    /// <summary>
    /// Имя пользователя для аутентификации
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Пароль для аутентификации
    /// </summary>
    public string Password { get; set; } = string.Empty;
    
    /// <summary>
    /// Отображаемое имя отправителя в email сообщениях
    /// </summary>
    public string SenderName { get; set; } = string.Empty;

    /// <summary>
    /// Email адрес отправителя для всех исходящих сообщений
    /// </summary>
    public string SenderEmail { get; set; } = string.Empty;

    /// <summary>
    /// Список email адресов администраторов библиотеки для
    /// </summary>
    public List<string> AdminEmails { get; set; } = new();
}