using PracticalWork.Reports.Abstractions.Services.Domain;
using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Services;

public class ReportService: IReportService
{
    public Task<Report> Generate(GenerateReportDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<Report>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<string> GetDownloadUrl(string reportName)
    {
        throw new NotImplementedException();
    }
}