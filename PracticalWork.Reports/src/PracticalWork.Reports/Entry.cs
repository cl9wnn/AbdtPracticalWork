using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PracticalWork.Reports.Abstractions.Services.Domain;
using PracticalWork.Reports.Events.Books;
using PracticalWork.Reports.Events.Books.Archive;
using PracticalWork.Reports.Events.Books.Borrow;
using PracticalWork.Reports.Events.Books.Create;
using PracticalWork.Reports.Events.Books.Return;
using PracticalWork.Reports.Events.Readers.Close;
using PracticalWork.Reports.Events.Readers.Create;
using PracticalWork.Reports.Models;
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
        services.AddScoped<ICsvExportService<ActivityLog>, ActivityLogCsvExportService>();
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