namespace PracticalWork.Reports.Abstractions.Services.Domain;

/// <summary>
/// Контракт сервиса генерации Csv файла из набора записей (каждая запись — одна строка, свойства — колонки)
/// </summary>
/// <typeparam name="T">Тип данных для генерации</typeparam>
public interface ITabularCsvExportService<T>
{
    /// <summary>
    /// Генерация csv файла 
    /// </summary>
    /// <param name="data">Данные для генерации файла</param>
    /// <returns>Массив байтов со сгенерированным файлом</returns>
    byte[] Generate(IEnumerable<T> data);
}