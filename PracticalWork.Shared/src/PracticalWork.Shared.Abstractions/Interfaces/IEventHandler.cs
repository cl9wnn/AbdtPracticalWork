using PracticalWork.Shared.Abstractions.Events;

namespace PracticalWork.Shared.Abstractions.Interfaces;

/// <summary>
/// Обработчик события из брокера сообщений
/// </summary>
/// <typeparam name="TEvent">Тип конкретного события</typeparam>
public interface IEventHandler<in TEvent> where TEvent: BaseEvent
{
    /// <summary>
    /// Обработка события из брокера сообщений
    /// </summary>
    /// <param name="event">Событие</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
}