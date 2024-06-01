using CommonLibrary;
using Confluent.Kafka;
using KafkaLibrary.Interfaces;
using SharedLibrary;
using System.Diagnostics;
using System.Net;

namespace KafkaLibrary.Implementations
{
    public class Producer : IProducer, IDisposable
    {
        private IProducer<string, BaseRequest> requestProducer;
        private IProducer<string, BaseResponse> responseProducer;
        private Stopwatch stopwatch;

        public Producer(ProducerConfig config)
        {
            requestProducer = new ProducerBuilder<string, BaseRequest>(config)
                .SetValueSerializer(new JsonSerializer<BaseRequest>())
                .Build();

            responseProducer = new ProducerBuilder<string, BaseResponse>(config)
                .SetValueSerializer(new JsonSerializer<BaseResponse>())
                .Build();

            stopwatch = new Stopwatch();
        }

        public void Dispose()
        {
            requestProducer.Flush(TimeSpan.FromSeconds(10));
            requestProducer.Dispose();

            responseProducer.Flush(TimeSpan.FromSeconds(10));
            responseProducer.Dispose();
        }

        public async Task<BaseResponse> ProduceAsync<T>(Topics topic, string key, T message) where T : class
        {
            stopwatch.Start();
            try
            {
                DeliveryResult<string, T>? result;

                if (message is BaseRequest baseRequest)
                {
                    var requestResult = await requestProducer.ProduceAsync(EnumHelper.GetDescription(topic),
                        new Message<string, BaseRequest> { Key = key, Value = baseRequest });
                    result = requestResult as DeliveryResult<string, T>;

                    stopwatch.Stop();

                    CacheOperation(key, baseRequest);

                    //await for response
                    return null;
                }
                else if (message is BaseResponse baseResponse)
                {
                    var responseResult = await responseProducer.ProduceAsync(EnumHelper.GetDescription(topic),
                        new Message<string, BaseResponse> { Key = key, Value = baseResponse });
                    result = responseResult as DeliveryResult<string, T>;

                    stopwatch.Stop();
                    return baseResponse;
                }
                else
                {
                    throw new ArgumentException("Unsupported message type.");
                }
            }
            catch (Exception)
            {
                stopwatch.Stop();
                throw;
            }
        }

        private void CacheOperation(string key, BaseRequest message)
        {
            BaseRequest cacheMessage = message;
            cacheMessage.HandlerMethod = "Save";
            requestProducer.Produce(EnumHelper.GetDescription(Topics.Cache), new Message<string, BaseRequest> { Key = key, Value = message });
        }
    }
}