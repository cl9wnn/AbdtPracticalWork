namespace PracticalWork.Reports.Abstractions.Services.Domain;

/// <summary>
/// Контракт сервиса генерации Csv файла
/// </summary>
/// <typeparam name="T">Тип данных для генерации</typeparam>
public interface ICsvExportService<T>
{
    /// <summary>
    /// Генерация csv файла 
    /// </summary>
    /// <param name="data">Данные для генерации файла</param>
    /// <returns>Массив байтов со сгенерированным файлом</returns>
    byte[] Generate(IEnumerable<T> data);
}