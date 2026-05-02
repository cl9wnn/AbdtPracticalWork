using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Abstractions.Storage;

/// <summary>
/// Контракт репозитория для работы с отчетами
/// </summary>
public interface IReportRepository
{
    /// <summary>
    /// Добавление информации об отчете в БД
    /// </summary>
    /// <param name="report">Информация об отчете</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Идентификатор добавленного отчета</returns>
    Task<Guid> Add(Report report, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получение всех отчетов 
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список отчетов</returns>
    Task<IReadOnlyList<Report>> GetAll(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получение информации об отчете по наименованию
    /// </summary>
    /// <param name="reportName">Наименование отчета</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Информация об отчете</returns>
    Task<Report> GetByName(string reportName, CancellationToken cancellationToken = default);
}