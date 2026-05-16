using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PracticalWork.Library.Abstractions.Services.Infrastructure;

namespace PracticalWork.Library.Clients.Http;

public static class Entry
{
    /// <summary>
    /// Регистрация зависимостей для HTTP-клиентов
    /// </summary>
    public static IServiceCollection AddReportsHttpClient(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddHttpClient<IReportsApiClient, ReportsApiClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["App:Services:Reports"]!);
        });
        
        return serviceCollection;
    }
}