using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Enums;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Abstractions.Storage;

/// <summary>
/// Контракт репозитория для работы с логами активности
/// </summary>
public interface IActivityLogRepository
{
    /// <summary>
    /// Добавление лога активности в БД
    /// </summary>
    /// <param name="activityLog">Лог активности</param>
    /// <param name="bookId">Идентификатор книги</param>
    /// <param name="readerId">Идентификатор карточки читателя</param>
    /// <returns></returns>
    Task<Guid> Add(ActivityLog activityLog, Guid? bookId = null, Guid? readerId = null);
    
    /// <summary>
    /// Получение логов активности
    /// </summary>
    /// <param name="filter">Фильтр поиска</param>
    /// <param name="pagination">Параметры пагинации</param>
    /// <returns>Список логов активности</returns>
    Task<IReadOnlyList<ActivityLog>> GetActivityLogs(ActivityLogFilterDto filter, PaginationDto pagination);
    
    /// <summary>
    /// Получение логов активности по типу события за определенный период
    /// </summary>
    /// <param name="from">Дата начала периода</param>
    /// <param name="to">Дата конца периода</param>
    /// <param name="eventType">Тип события</param>
    /// <returns></returns>
    Task<IReadOnlyList<ActivityLog>> GetActivityLogsByPeriodAsync(DateOnly from, DateOnly to,
        ActivityEventType eventType);
}