using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Events.Abstractions;

/// <summary>
/// Контракт события, позволяющий создавать лог активности
/// </summary>
public interface IActivityLoggable
{ 
    /// <summary>
    /// Создание лога активности из события брокера сообщений
    /// </summary>
    /// <returns>Лог активности</returns>
    ActivityLog ToActivityLog();
}