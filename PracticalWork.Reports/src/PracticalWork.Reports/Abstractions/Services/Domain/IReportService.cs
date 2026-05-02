using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Abstractions.Services.Domain;

/// <summary>
/// Контракт сервиса по работе с отчетами об активности
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Генерация отчета с данными об активности
    /// </summary>
    /// <param name="dto">Модель с данными для генерации отчета</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Информация о сгенерированном отчете</returns>
    Task<Report> GenerateActivityLogsReport(GenerateActivityLogReportDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Генерация отчета с еженедельной статистикой
    /// </summary>
    /// <param name="dto">Модель с данными для генерации отчета</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Информация о сгенерированном отчете</returns>
    Task<Report> GenerateWeeklyStatisticsReport(GenerateWeeklyReportDto dto, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получение всех отчетов
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список с информацией об имеющихся отчетах</returns>
    Task<IReadOnlyCollection<Report>> GetAll(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получение ссылки для скачивания отчета
    /// </summary>
    /// <param name="reportName">Наименование отчета</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Signed URL отчета</returns>
    Task<string> GetDownloadUrl(string reportName, CancellationToken cancellationToken = default);
}