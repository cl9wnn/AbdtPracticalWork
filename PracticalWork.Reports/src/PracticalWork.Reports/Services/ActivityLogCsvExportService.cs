using System.Globalization;
using System.Text;
using System.Text.Json;
using CsvHelper;
using PracticalWork.Reports.Abstractions.Services.Domain;
using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Services;

/// <summary>
/// Сервис для генерации csv таблицы по логам активностей
/// </summary>
public sealed class ActivityLogCsvExportService
    : ICsvExportService<ActivityLog>
{
    public byte[] Generate(IEnumerable<ActivityLog> logs)
    {
        var rows = logs.Select(x => new ActivityLogReportRow
        {
            EventDate = x.EventDate,
            EventType = x.EventType.ToString(),
            Metadata = x.Metadata.RootElement.GetRawText()
        });
        
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream, Encoding.UTF8);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteField("Тип события");
        csv.WriteField("Дата события");
        csv.WriteField("Дополнительная информация");
        csv.NextRecord();

        foreach (var log in rows)
        {
            csv.WriteField(log.EventType);
            csv.WriteField(log.EventDate.ToString("yyyy-MM-dd HH:mm:ss"));
            csv.WriteField(log.Metadata);
            csv.NextRecord();
        }

        writer.Flush();
        return memoryStream.ToArray();
    }
}
