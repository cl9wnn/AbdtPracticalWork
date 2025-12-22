using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Abstractions.Services.Domain;

public interface IReportService
{
    Task<Report> Generate(GenerateReportDto dto);
    Task<IReadOnlyCollection<Report>> GetAll();
    Task<string> GetDownloadUrl(string reportName);
}