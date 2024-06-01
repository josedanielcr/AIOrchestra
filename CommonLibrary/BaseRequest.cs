using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class BaseRequest : BaseContract
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string RequesterId { get; set; } = string.Empty;
        public string CorrelationId { get; set; } = Guid.NewGuid().ToString();
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public Topics TargetTopic { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
    }
}
