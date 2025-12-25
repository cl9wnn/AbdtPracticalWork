namespace PracticalWork.Reports.SharedKernel.Events;

/// <summary>
///  Реестр для сопоставления строковых типов событий с типами .NET
/// </summary>
public interface IEventTypeRegistry
{
    /// <summary>
    /// Возвращает тип события по его строковому типу из JSON
    /// </summary>
    /// <param name="eventType">Строковый тип события</param>
    /// <returns></returns>
    Type GetEventType(string eventType);
}