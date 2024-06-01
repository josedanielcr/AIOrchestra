using CommonLibrary;

namespace KafkaLibrary.Interfaces
{
    public interface IProducer : IDisposable
    {
        Task<BaseResponse> ProduceAsync<T>(Topics topic, string key, T message) where T : class;
        new void Dispose();
    }
}