using System.Globalization;
using System.Text;
using CsvHelper;
using PracticalWork.Reports.Abstractions.Services.Domain;
using PracticalWork.Reports.Dtos;

namespace PracticalWork.Reports.Services;

/// <summary>
/// Сервис для генерации csv таблицы по еженедельной статистике 
/// </summary>
public class WeeklyStatisticsCsvExportService : IKeyValueCsvExportService<WeeklyStatisticsDto>
{
    /// <inheritdoc cref="IKeyValueCsvExportService{T}.Generate"/>
    public byte[] Generate(WeeklyStatisticsDto stats)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream, Encoding.UTF8);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        
        csv.WriteField("Метрика");
        csv.WriteField("Значение");
        csv.NextRecord();

        csv.WriteField(nameof(stats.NewBooksCount));
        csv.WriteField(stats.NewBooksCount);
        csv.NextRecord();

        csv.WriteField(nameof(stats.NewReadersCount));
        csv.WriteField(stats.NewReadersCount);
        csv.NextRecord();

        csv.WriteField(nameof(stats.BorrowedBooksCount));
        csv.WriteField(stats.BorrowedBooksCount);
        csv.NextRecord();

        csv.WriteField(nameof(stats.ReturnedBooksCount));
        csv.WriteField(stats.ReturnedBooksCount);
        csv.NextRecord();

        csv.WriteField(nameof(stats.OverdueBooksCount));
        csv.WriteField(stats.OverdueBooksCount);
        csv.NextRecord();

        writer.Flush();
        return memoryStream.ToArray();
    }
}