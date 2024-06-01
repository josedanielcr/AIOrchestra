using CommonLibrary;
using Confluent.Kafka;
using KafkaLibrary.Interfaces;
using SharedLibrary;

namespace AIOrchestra.UserManagementService.Shared
{
    public class UpdateCacheOperation
    {
        private readonly IProducer producer;

        public UpdateCacheOperation(IProducer producer)
        {
            this.producer = producer;
        }
        public async void UpdateCache(BaseRequest request)
        {
            //request.HandlerMethod = "SaveResponseInToCache";
            //await producer.ProduceAsync(EnumHelper.GetDescription(Topics.Cache), new Message<string, BaseRequest> { Key = key, Value = message });
        }
    }
}