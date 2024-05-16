using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class BaseResponse : BaseContract
    {
        public bool IsSuccess { get; set; } = true;
        public bool IsFailure { get; set; } = false;
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public Error Error { get; set; } = new Error();
        public long ProcessingTime { get; set; }
        public Topics ServicedBy { get; set; }
        public Dictionary<string, string> AdditionalDetails { get; set; } = new Dictionary<string, string>();
    }

    public class Error
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }
}
