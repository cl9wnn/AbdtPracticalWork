using PracticalWork.Reports.Contracts.v1.Enums;

namespace PracticalWork.Reports.Contracts.v1.Abstracts;

/// <summary>
/// Базовый контракт для отчета
/// </summary>
/// <param name="Name">Название отчета</param>
/// <param name="FilePath">Путь к файлу MinIO</param>
/// <param name="Status">Статус отчета</param>
/// <param name="GeneratedAt">Дата и время генерации</param>
public abstract record AbstractReport(string Name, string FilePath, ReportStatus Status, DateTime GeneratedAt);