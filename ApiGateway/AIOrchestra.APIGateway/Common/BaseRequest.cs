﻿namespace AIOrchestra.APIGateway.Common
{
    public class BaseRequest : BaseContract
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string RequesterId { get; set; } = string.Empty;
        public int TimeoutInSeconds { get; set; } = 30;
    }
}