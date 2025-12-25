using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Abstractions.Services.Domain;

/// <summary>
/// Контракт сервиса по работе с отчетами об активности
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Генерация отчета
    /// </summary>
    /// <param name="dto">Модель с данными для генерации отчета</param>
    /// <returns>Информация о сгенерированном отчете</returns>
    Task<Report> Generate(GenerateReportDto dto);
    
    /// <summary>
    /// Получение всех отчетов
    /// </summary>
    /// <returns>Список с информацией об имеющихся отчетах</returns>
    Task<IReadOnlyCollection<Report>> GetAll();
    
    /// <summary>
    /// Получение ссылки для скачивания отчета
    /// </summary>
    /// <param name="reportName">Наименование отчета</param>
    /// <returns>Signed URL отчета</returns>
    Task<string> GetDownloadUrl(string reportName);
}