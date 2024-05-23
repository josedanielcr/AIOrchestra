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
        private readonly string CONSUMER_STOPPED = "CONSUMER_STOPPED";
        private readonly string CONSUMER_STOPPED_MESSAGE = "Consumer stopped due to an exception";
        public Consumer(ConsumerConfig config)
        {
            consumer = new ConsumerBuilder<string, BaseRequest>(config)
                .SetValueDeserializer(new JsonSerializer<BaseRequest>())
                .Build();
        }

        public async Task<BaseResponse> Consume(Topics topic)
        {
            keepConsuming = true;
            consumer.Subscribe(topic.ToString());
            var consumeResult = consumer.Consume();
            while (keepConsuming)
            {
                return await ExecuteMessageMethodHandler(consumeResult);
            }
            consumer.Close();
            return GenerateApplicationResponse.GenerateResponse(
                null,
                null,
                isSuccess: false,
                HttpStatusCode.BadRequest,
                CONSUMER_STOPPED,
                CONSUMER_STOPPED_MESSAGE,
                null,
                null,
                null,
                null);
        }

        private async Task<BaseResponse> ExecuteMessageMethodHandler(ConsumeResult<string, BaseRequest> consumeResult)
        {
            var result = await InvokeMethodHelper.InvokeMethod(consumeResult.Message.Value);
            return result;
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