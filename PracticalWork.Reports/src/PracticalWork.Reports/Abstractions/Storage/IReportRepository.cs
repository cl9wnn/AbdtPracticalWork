using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Abstractions.Storage;

public interface IReportRepository
{
    Task<Guid> Add(Report report);
    Task<IReadOnlyList<Report>> GetAll();
    Task<Report> GetByName(string reportName);
}