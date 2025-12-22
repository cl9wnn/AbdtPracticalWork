using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PracticalWork.Reports.Abstractions.Services.Domain;
using PracticalWork.Reports.Options;
using PracticalWork.Reports.Services;

namespace PracticalWork.Reports;

public static class Entry
{
    /// <summary>
    /// Регистрация зависимостей уровня бизнес-логики
    /// </summary>
    public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BooksCacheOptions>(configuration.GetSection("App:BooksCache"));

        services.AddScoped<IActivityLogService, ActivityLogService>();
        services.AddScoped<IReportService, ReportService>();
        
        return services;
    }
}