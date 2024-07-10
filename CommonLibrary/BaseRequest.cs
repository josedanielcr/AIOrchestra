namespace CommonLibrary
{
    public class BaseRequest : BaseContract
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string RequesterId { get; set; } = string.Empty;
        public string CorrelationId { get; set; } = Guid.NewGuid().ToString();
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public Topics TargetTopic { get; set; }
    }
}