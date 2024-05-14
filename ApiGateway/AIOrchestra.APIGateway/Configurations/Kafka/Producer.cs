using AIOrchestra.APIGateway.Kafka.Producers;
using Confluent.Kafka;

namespace AIOrchestra.APIGateway.Configurations.Kafka
{
    public static class Producer
    {
        public static IServiceCollection AddKafkaProducer(this IServiceCollection services, IConfiguration configuration)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                ClientId = configuration["Kafka:ClientId"],
                Acks = Acks.All
            };
            services.AddSingleton<IProducerService, ProducerService>(provider =>
                new ProducerService(producerConfig));
            return services;
        }
    }
}
