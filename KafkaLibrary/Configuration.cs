using Confluent.Kafka;
using KafkaLibrary.Implementations;
using KafkaLibrary.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KafkaLibrary
{
    public static class Configuration
    {
        public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                ClientId = configuration["Kafka:ClientId"],
                Acks = Acks.All
            };

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                ClientId = configuration["Kafka:ClientId"],
                GroupId = configuration["Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            services.AddSingleton<IConsumer, Consumer>(provider =>
                new Consumer(consumerConfig));


            services.AddSingleton<IProducer, Producer>(provider =>
            {
                var consumer = provider.GetRequiredService<IConsumer>();
                return new Producer(producerConfig, consumer);
            });

            return services;
        }
    }
}
