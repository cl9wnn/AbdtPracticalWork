using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PracticalWork.Library.Abstractions.Services.Domain;
using PracticalWork.Library.Options;
using PracticalWork.Library.Services;

namespace PracticalWork.Library;

public static class Entry
{
    /// <summary>
    /// Регистрация зависимостей уровня бизнес-логики
    /// </summary>
    public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BooksCacheOptions>(configuration.GetSection("App:BooksCache"));
        services.Configure<ReadersCacheOptions>(configuration.GetSection("App:ReadersCache"));

        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IReaderService, ReaderService>();
        services.AddScoped<ILibraryService, LibraryService>();
        
        return services;
    }
}