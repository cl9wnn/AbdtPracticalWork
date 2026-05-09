using PracticalWork.Shared.Abstractions.Events;

namespace PracticalWork.Library.Abstractions.Services.Infrastructure;

/// <summary>
/// Контракт продюсера брокера сообщений
/// </summary>
public interface IMessageBrokerProducer : IDisposable
{
    /// <summary>
    /// Отправляет сообщение в брокер сообщений.
    /// </summary>
    /// <param name="key">Тип события</param>
    /// <param name="message">Сообщение (событие), которое необходимо отправить в брокер</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <typeparam name="TEvent"></typeparam>
    Task ProduceAsync<TEvent>(string key, TEvent message, CancellationToken cancellationToken = default)
        where TEvent : BaseEvent;
}