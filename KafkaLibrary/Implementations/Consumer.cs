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
        private IConsumer<string, BaseResponse> responseConsumer;
        private bool keepConsuming = true;
        public Consumer(ConsumerConfig config)
        {
            consumer = new ConsumerBuilder<string, BaseRequest>(config)
                .SetValueDeserializer(new JsonSerializer<BaseRequest>())
                .Build();

            responseConsumer = new ConsumerBuilder<string, BaseResponse>(config)
                .SetValueDeserializer(new JsonSerializer<BaseResponse>())
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

        public BaseResponse ConsumeResponse(Topics topic)
        {
            keepConsuming = true;
            responseConsumer.Subscribe(EnumHelper.GetDescription(topic));
            var consumeResult = responseConsumer.Consume();
            while (keepConsuming)
            {
                return consumeResult.Message.Value;
            }
            consumer.Close();
            return new BaseResponse { };
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