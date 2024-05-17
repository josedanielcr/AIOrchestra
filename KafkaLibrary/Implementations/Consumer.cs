using CommonLibrary;
using Confluent.Kafka;
using KafkaLibrary.Interfaces;
using SharedLibrary;

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

        public BaseRequest Consume(Topics topic /*send a function somehow to know what to process after getting the response*/)
        {
            keepConsuming = true;
            consumer.Subscribe(topic.ToString());
            while (keepConsuming)
            {
                var consumeResult = consumer.Consume();
                if (consumeResult != null)
                {
                    //call the parameter function here
                }
            }
            consumer.Close();
            return null!;
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
