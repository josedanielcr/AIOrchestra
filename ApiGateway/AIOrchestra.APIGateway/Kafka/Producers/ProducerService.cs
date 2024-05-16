using AIOrchestra.APIGateway.Common.Entities;
using AIOrchestra.APIGateway.Common.Enums;
using AIOrchestra.APIGateway.Helpers;
using Confluent.Kafka;
using System.Diagnostics;
using System.Net;

namespace AIOrchestra.APIGateway.Kafka.Producers
{
    public class ProducerService : IProducerService
    {
        private IProducer<string, BaseRequest> producer;
        private Stopwatch stopwatch;

        public ProducerService(ProducerConfig config)
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
                var result = await producer.ProduceAsync(Helpers.EnumHelper.GetDescription(topic),
                    new Message<string, BaseRequest> { Key = key, Value = message });

                stopwatch.Stop();

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
    }
}