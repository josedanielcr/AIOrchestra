using CommonLibrary;
using Confluent.Kafka;
using KafkaLibrary.Interfaces;
using SharedLibrary;
using System.Net;

namespace KafkaLibrary.Implementations
{
    public class Consumer : IConsumer
    {
        private IConsumer<string, BaseRequest> consumer;
        private bool keepConsuming = true;
        public Consumer(ConsumerConfig config)
        {
            consumer = new ConsumerBuilder<string, BaseRequest>(config)
                .SetValueDeserializer(new JsonSerializer<BaseRequest>())
                .Build();
        }

        public BaseRequest Consume(Topics topic)
        {
            keepConsuming = true;
            consumer.Subscribe(EnumHelper.GetDescription(topic));
            var consumeResult = consumer.Consume();
            while (keepConsuming)
            {
                return consumeResult.Message.Value;
            }
            consumer.Close();
            return new BaseRequest { };
        }

        public void StopConsuming()
        {
            keepConsuming = false;
        }

        public void Dispose()
        {
            consumer.Dispose();
        }
    }
}