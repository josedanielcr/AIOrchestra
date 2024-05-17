using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaLibrary.Interfaces
{
    public interface IProducer : IDisposable
    {
        Task<BaseResponse> ProduceAsync(Topics topic, string key, BaseRequest message);
        new void Dispose();
    }
}
