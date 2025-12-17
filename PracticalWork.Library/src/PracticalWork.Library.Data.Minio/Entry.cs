using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PracticalWork.Library.Abstractions.Services.Infrastructure;

namespace PracticalWork.Library.Data.Minio;

public static class Entry
{
    /// <summary>
    /// Регистрация зависимостей для хранилища документов
    /// </summary>
    public static IServiceCollection AddMinioFileStorage(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.Configure<MinioOptions>(configuration.GetSection("App:Minio"));

        serviceCollection.AddSingleton<IFileStorageService, MinioFileStorageService>();

        return serviceCollection;
    }
}