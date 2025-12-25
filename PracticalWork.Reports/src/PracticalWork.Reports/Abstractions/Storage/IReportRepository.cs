using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Abstractions.Storage;

/// <summary>
/// Контракт репозитория для работы с 
/// </summary>
public interface IReportRepository
{
    /// <summary>
    /// Добавление информации об отчете в БД
    /// </summary>
    /// <param name="report">Информация об отчете</param>
    /// <returns>Идентификатор добавленного отчета</returns>
    Task<Guid> Add(Report report);
    
    /// <summary>
    /// Получение всех отчетов 
    /// </summary>
    /// <returns>Список отчетов</returns>
    Task<IReadOnlyList<Report>> GetAll();
    
    /// <summary>
    /// Получение информации об отчете по наименованию
    /// </summary>
    /// <param name="reportName">Наименование отчета</param>
    /// <returns>Информация об отчете</returns>
    Task<Report> GetByName(string reportName);
}