namespace PracticalWork.Reports.Abstractions.Services.Domain;

public interface ICsvExportService<T>
{
    byte[] Generate(IEnumerable<T> rows);
}