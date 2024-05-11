using System;
using Microsoft.Extensions.Configuration;

namespace ApiGateway.Common
{
    public class BaseRequest : BaseContract
    {
        private readonly IConfiguration _configuration;
        private readonly int defaultTimeoutInSeconds = 30;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string RequesterId { get; set; } = string.Empty;

        public int TimeoutInSeconds { get; set; }

        public BaseRequest(IConfiguration configuration)
        {
            _configuration = configuration;
            TimeoutInSeconds = _configuration.GetValue("OperationConstants:TimeoutInSeconds", defaultTimeoutInSeconds);
        }
    }
}