using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PracticalWork.Reports.Abstractions.Services.Domain;
using PracticalWork.Reports.Dtos;
using PracticalWork.Reports.Features.Books.Create;
using PracticalWork.Reports.Features.Books.Return;
using PracticalWork.Reports.Features.Readers.Close;
using PracticalWork.Reports.Features.Readers.Create;
using PracticalWork.Reports.Features.Books.Archive;
using PracticalWork.Reports.Features.Books.Borrow;
using PracticalWork.Reports.Models;
using PracticalWork.Reports.Options.Cache;
using PracticalWork.Reports.Services;
using PracticalWork.Shared.Abstractions.Interfaces;
using PracticalWork.Shared.Contracts.Events.Books;
using PracticalWork.Shared.Contracts.Events.Readers;

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
        services.AddScoped<ITabularCsvExportService<ActivityLog>, ActivityLogCsvExportService>();
        services.AddScoped<IKeyValueCsvExportService<WeeklyStatisticsDto>, WeeklyStatisticsCsvExportService>();
        services.AddScoped<IReportService, ReportService>();
        
        services.AddScoped<IEventHandler<BookCreatedEvent>, BookCreatedEventHandler>();
        services.AddScoped<IEventHandler<BookArchivedEvent>, BookArchivedEventHandler>();
        services.AddScoped<IEventHandler<BookBorrowedEvent>, BookBorrowedEventHandler>();
        services.AddScoped<IEventHandler<BookReturnedEvent>, BookReturnedEventHandler>();
        services.AddScoped<IEventHandler<ReaderClosedEvent>, ReaderClosedEventHandler>();
        services.AddScoped<IEventHandler<ReaderCreatedEvent>, ReaderCreatedEventHandler>();
        
        return services;
    }
}