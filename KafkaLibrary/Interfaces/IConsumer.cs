using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaLibrary.Interfaces
{
    public interface IConsumer : IDisposable
    {
        BaseRequest Consume(Topics topic);
        BaseResponse ConsumeResponse(Topics topic);
        void StopConsuming();
    }
}
