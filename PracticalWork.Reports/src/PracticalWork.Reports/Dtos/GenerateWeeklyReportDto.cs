namespace PracticalWork.Reports.Dtos;

/// <summary>
/// DTO для генерации еженедельного отчета со статистикой
/// </summary>
public class GenerateWeeklyReportDto
{
    /// <summary>
    /// Начало периода отчетности
    /// </summary>
    public DateOnly PeriodFrom { get; set; }
    
    /// <summary>
    /// Конец периода отчетности
    /// </summary>
    public DateOnly PeriodTo { get; set; }
    
    /// <summary>
    /// Данные с еженедельной статистикой
    /// </summary>
    public WeeklyStatisticsDto WeeklyStatistics { get; set; }
}