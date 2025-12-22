using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Abstractions.Services.Domain;

public interface IActivityLogService
{ 
    Task<PageDto<ActivityLog>> GetPagedActivityLogs(ActivityLogFilterDto filter, PaginationDto pagination);
}