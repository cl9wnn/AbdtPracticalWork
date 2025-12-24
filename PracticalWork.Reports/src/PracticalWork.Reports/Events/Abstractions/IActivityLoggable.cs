using PracticalWork.Reports.Models;

namespace PracticalWork.Reports.Events.Abstractions;

public interface IActivityLoggable
{ 
    ActivityLog ToActivityLog();
}