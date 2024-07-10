using CacheLibrary.Interfaces;
using CommonLibrary;
using Confluent.Kafka;
using KafkaLibrary.Interfaces;
using SharedLibrary;
using System.Diagnostics;

namespace KafkaLibrary.Implementations
{
    public class Producer : IProducer, IDisposable
    {
        private readonly ICacheUtils cacheUtils;
        private IProducer<string, BaseRequest> requestProducer;
        private Stopwatch stopwatch;
        private TimeSpan TimeSpan = TimeSpan.FromSeconds(10);

        public Producer(ProducerConfig config, ICacheUtils cache)
        {
            requestProducer = new ProducerBuilder<string, BaseRequest>(config)
                .SetValueSerializer(new JsonSerializer<BaseRequest>())
                .Build();

            stopwatch = new Stopwatch();
            cacheUtils = cache;
        }

        public void Dispose()
        {
            requestProducer.Flush(TimeSpan.FromSeconds(10));
            requestProducer.Dispose();
        }

        public async Task<BaseResponse> ProduceAsync<T>(Topics topic, string key, T message) where T : class
        {
            stopwatch.Start();
            try
            {
                if (message is BaseRequest baseRequest)
                {
                    await requestProducer.ProduceAsync(EnumHelper.GetDescription(topic),
                        new Message<string, BaseRequest> { Key = key, Value = baseRequest });

                    BaseResponse response = await WaitForServiceResponse(baseRequest.OperationId, TimeSpan, HasServiceResponded);
                    stopwatch.Stop();
                    response.ProcessingTime = stopwatch.ElapsedMilliseconds;
                    return response;
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

        private async Task<BaseResponse> WaitForServiceResponse(string operationId, TimeSpan timeout, Func<string, Task<(bool, BaseResponse)>> conditionChecker)
        {
            using CancellationTokenSource cts = new CancellationTokenSource(timeout);

            while (!cts.Token.IsCancellationRequested)
            {
                (bool conditionMet, BaseResponse result) = await conditionChecker(operationId);
                if (conditionMet)
                {
                    return result;
                }

                // Delay to prevent busy-waiting
                await Task.Delay(100, cts.Token);
            }

            throw new TimeoutException("Result was not provided by the service");
        }

        private async Task<(bool, BaseResponse)> HasServiceResponded(string operationId)
        {
            var result = await cacheUtils.Get<BaseResponse>(operationId);
            if (result == null) return (false, null!);
            if (result.Status == RequestStatus.Pending) return (true, null!);
            return (true, result);
        }
    }
}