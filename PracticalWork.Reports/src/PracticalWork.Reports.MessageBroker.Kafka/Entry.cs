using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PracticalWork.Reports.Events;
using PracticalWork.Reports.SharedKernel.Events;

namespace PracticalWork.Reports.MessageBroker.Kafka;

public static class Entry
{
    public static IServiceCollection AddKafkaConsumers(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        
        serviceCollection.Configure<KafkaOptions>(configuration.GetSection("App:Kafka"));
        
        serviceCollection.AddSingleton<IEventTypeRegistry, EventTypeRegistry>();
        serviceCollection.AddHostedService<KafkaConsumer>(); 
        return serviceCollection;
    }
}