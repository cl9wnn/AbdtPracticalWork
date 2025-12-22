using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PracticalWork.Reports.Abstractions.Services.Domain;
using PracticalWork.Reports.Events.Books.Archive;
using PracticalWork.Reports.Events.Books.Create;
using PracticalWork.Reports.Options;
using PracticalWork.Reports.Services;
using PracticalWork.Reports.SharedKernel.Abstractions;

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
        
        services.AddScoped<IEventHandler<BookCreatedEvent>, BookCreatedEventHandler>();
        services.AddScoped<IEventHandler<BookArchivedEvent>, BookArchivedEventHandler>();

        return services;
    }
}