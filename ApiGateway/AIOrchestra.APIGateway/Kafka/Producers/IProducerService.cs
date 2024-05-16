
using CommonLibrary;

namespace AIOrchestra.APIGateway.Kafka.Producers
{
    public interface IProducerService : IDisposable
    {
        Task<BaseResponse> ProduceAsync(Topics topic, string key, BaseRequest message);
        new void Dispose();
    }
}
