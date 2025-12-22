using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PracticalWork.Library.Abstractions.Services.Infrastructure;
using PracticalWork.Library.SharedKernel.Events;

namespace PracticalWork.Library.MessageBroker.Kafka;

public static class Entry
{
    public static IServiceCollection AddKafkaProducer(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.Configure<KafkaOptions>(configuration.GetSection("App:Kafka"));
        
        serviceCollection.AddSingleton<IMessageBrokerProducer, KafkaProducer>();

        return serviceCollection;
    }
}