using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class BaseContract
    {
        public string OperationId { get; set; } = Guid.NewGuid().ToString();
        public string ApiVersion { get; set; } = string.Empty;
        public object Value { get; set; } = null!;
        public string HandlerMethod { get; set; } = string.Empty;
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
    }
}
