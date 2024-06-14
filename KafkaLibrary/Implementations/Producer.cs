using CommonLibrary;
using Confluent.Kafka;
using KafkaLibrary.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedLibrary;
using System.Diagnostics;
using System.Dynamic;

namespace KafkaLibrary.Implementations
{
    public class Producer : IProducer, IDisposable
    {
        private readonly IConsumer consumer;
        private IProducer<string, BaseRequest> requestProducer;
        private IProducer<string, BaseResponse> responseProducer;
        private Stopwatch stopwatch;

        public Producer(ProducerConfig config, IConsumer consumer)
        {
            requestProducer = new ProducerBuilder<string, BaseRequest>(config)
                .SetValueSerializer(new JsonSerializer<BaseRequest>())
                .Build();

            responseProducer = new ProducerBuilder<string, BaseResponse>(config)
                .SetValueSerializer(new JsonSerializer<BaseResponse>())
                .Build();

            stopwatch = new Stopwatch();
            this.consumer = consumer;
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
                if (message is BaseRequest baseRequest)
                {
                    await requestProducer.ProduceAsync(EnumHelper.GetDescription(topic),
                        new Message<string, BaseRequest> { Key = key, Value = baseRequest });


                    BaseResponse response = WaitForServiceResponse(baseRequest.OperationId);
                    stopwatch.Stop();
                    response.ProcessingTime = stopwatch.ElapsedMilliseconds;
                    return response;
                }
                else if (message is BaseResponse baseResponse)
                {
                    await responseProducer.ProduceAsync(EnumHelper.GetDescription(topic),
                        new Message<string, BaseResponse> { Key = key, Value = baseResponse });
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

        private BaseResponse WaitForServiceResponse(string operationId)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            try
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    var result = consumer.ConsumeResponse(Topics.ApiGatewayResponse);
                    if (result.OperationId == operationId)
                    {
                        consumer.StopConsuming();
                        return ManageTopicResult(result);
                    }
                }
                throw new TimeoutException("The operation timed out.");
            }
            catch (OperationCanceledException)
            {
                throw new TimeoutException("The operation was canceled.");
            }
        }

        private BaseResponse ManageTopicResult(BaseResponse result)
        {
            if (result.Value == null)
            {
                return result;
            }

            if (result.Value is IEnumerable<JToken> jTokens)
            {
                var value = ConvertJTokenEnumerableToObject(jTokens);
                result.Value = value;
            }
            else
            {
                var singleValue = ConvertJTokenToObject((JToken)result.Value);
                result.Value = singleValue;
            }

            return result;
        }

        private object ConvertJTokenEnumerableToObject(IEnumerable<JToken> tokens)
        {
            dynamic expando = new ExpandoObject();
            var expandoDict = (IDictionary<string, object>)expando;
            var obj_linq = tokens.Cast<KeyValuePair<string, JToken>>();

            foreach (var kvp in obj_linq)
            {
                expandoDict[kvp.Key] = ConvertJTokenToObject(kvp.Value);
            }

            return expando;
        }

        private object ConvertJTokenToObject(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    return token.ToObject<Dictionary<string, object>>()!;
                case JTokenType.Array:
                    return token.ToObject<List<object>>()!;
                default:
                    return ((JValue)token).Value!;
            }
        }
    }
}