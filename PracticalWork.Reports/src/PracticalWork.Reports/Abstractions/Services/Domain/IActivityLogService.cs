using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Abstractions.Services.Domain;

/// <summary>
/// Контракт сервиса по работе с логами активностей
/// </summary>
public interface IActivityLogService
{ 
    /// <summary>
    /// Получение отфильтрованных логов активностей с пагинацией
    /// </summary>
    /// <param name="filter">Фильтр</param>
    /// <param name="pagination">Пагинация</param>
    /// <returns>Страница с отфильтрованными логами активности</returns>
    Task<PageDto<ActivityLog>> GetPagedActivityLogs(ActivityLogFilterDto filter, PaginationDto pagination);
}