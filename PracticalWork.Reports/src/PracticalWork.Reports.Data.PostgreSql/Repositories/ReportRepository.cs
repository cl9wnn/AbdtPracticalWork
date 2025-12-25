using Microsoft.EntityFrameworkCore;
using PracticalWork.Reports.Abstractions.Storage;
using PracticalWork.Reports.Data.PostgreSql.Entities;
using PracticalWork.Reports.Data.PostgreSql.Mappers.v1;
using PracticalWork.Reports.Enums;
using PracticalWork.Reports.Exceptions;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Data.PostgreSql.Repositories;

/// <summary>
/// 
/// </summary>
public class ReportRepository : IReportRepository
{
    private readonly AppDbContext _appDbContext;

    public ReportRepository(AppDbContext dbContext)
    {
        _appDbContext = dbContext;
    }

    /// <inheritdoc cref="IReportRepository.Add"/>
    public async Task<Guid> Add(Report report)
    {
        var reportEntity = report.ToReportEntity();
        _appDbContext.Reports.Add(reportEntity);
        await _appDbContext.SaveChangesAsync();

        return reportEntity.Id;
    }

    /// <inheritdoc cref="IReportRepository.GetAll"/>
    public async Task<IReadOnlyList<Report>> GetAll()
    {
        return await _appDbContext.Reports
            .Select(r => r.ToReport())
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc cref="IReportRepository.GetByName"/>
    public async Task<Report> GetByName(string reportName)
    {
        var readerEntity = await _appDbContext.Reports
            .FirstOrDefaultAsync(r => r.Name == reportName);

        if (readerEntity == null)
        {
            throw new EntityNotFoundException("Отчет не найден по данному наименованию!");
        }

        return readerEntity.ToReport();
    }
}