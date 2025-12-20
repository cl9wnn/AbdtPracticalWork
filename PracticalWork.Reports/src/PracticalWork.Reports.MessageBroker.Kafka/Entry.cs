using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PracticalWork.Reports.MessageBroker.Kafka;

public static class Entry
{
    public static IServiceCollection AddKafkaConsumer<TEvent>(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        
        serviceCollection.Configure<KafkaOptions>(configuration.GetSection("App:Kafka"));
        
        serviceCollection.AddHostedService<KafkaConsumer<TEvent>>();
        
        return serviceCollection;
    }
}