using CommonLibrary;
using Confluent.Kafka;
using KafkaLibrary.Interfaces;
using SharedLibrary;
using System;
using System.Diagnostics;
using System.Net;

namespace KafkaLibrary.Implementations
{
    public class Producer : IProducer
    {
        private IProducer<string, BaseRequest> producer;
        private Stopwatch stopwatch;

        public Producer(ProducerConfig config)
        {
            producer = new ProducerBuilder<string, BaseRequest>(config)
                .SetValueSerializer(new JsonSerializer<BaseRequest>())
                .Build();
            stopwatch = new Stopwatch();
        }

        public void Dispose()
        {
            producer.Flush(TimeSpan.FromSeconds(10));
            producer.Dispose();
        }

        public async Task<BaseResponse> ProduceAsync(Topics topic, string key, BaseRequest message)
        {
            stopwatch.Start();
            try
            {
                var result = await producer.ProduceAsync(EnumHelper.GetDescription(topic),
                    new Message<string, BaseRequest> { Key = key, Value = message });

                stopwatch.Stop();

                CacheOperation(key, message);

                return GenerateApplicationResponse.GenerateResponse(
                    message.OperationId,
                    message.ApiVersion,
                    true,
                    HttpStatusCode.OK,
                    null,
                    null,
                    null,
                    stopwatch.ElapsedMilliseconds,
                    message.TargetTopic,
                    new Dictionary<string, string>
                {
                    {"Topic", result.Topic},
                    {"Partition", result.Partition.ToString()},
                    {"Offset", result.Offset.ToString()},
                });
            }
            catch (ProduceException<string, BaseRequest> e)
            {
                stopwatch.Stop();

                return GenerateApplicationResponse.GenerateResponse(
                    message.OperationId,
                    message.ApiVersion,
                    false,
                    HttpStatusCode.InternalServerError,
                    e.Error.Code.ToString(),
                    e.Message,
                    null,
                    stopwatch.ElapsedMilliseconds,
                    message.TargetTopic,
                    null);
            }
        }

        private void CacheOperation(string key, BaseRequest message)
        {
            producer.Produce(EnumHelper.GetDescription(Topics.Cache), new Message<string, BaseRequest> { Key = key, Value = message });
        }
    }
}