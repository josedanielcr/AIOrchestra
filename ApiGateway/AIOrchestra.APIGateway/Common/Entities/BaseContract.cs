namespace AIOrchestra.APIGateway.Common.Entities
{
    public class BaseContract
    {
        public string OperationId { get; set; } = Guid.NewGuid().ToString();
        public string ApiVersion { get; set; } = string.Empty;
    }
}