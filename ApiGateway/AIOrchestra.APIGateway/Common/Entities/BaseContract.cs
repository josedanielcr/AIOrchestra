namespace AIOrchestra.APIGateway.Common.Entities
{
    public class BaseContract
    {
        public Guid OperationId { get; set; } = new Guid();
        public string ApiVersion { get; set; } = string.Empty;
    }
}
