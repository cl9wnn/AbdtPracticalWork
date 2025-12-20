using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PracticalWork.Library.Abstractions.Services.Infrastructure;

namespace PracticalWork.Library.MessageBroker.Kafka;

public static class Entry
{
    public static IServiceCollection AddKafkaProducer<TMessage>(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.Configure<KafkaOptions>(configuration.GetSection("App:Kafka"));
        
        serviceCollection.AddSingleton<IMessageBrokerProducer<TMessage>, KafkaProducer<TMessage>>();

        return serviceCollection;
    }
}