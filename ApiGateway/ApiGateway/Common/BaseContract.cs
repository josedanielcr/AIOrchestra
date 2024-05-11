namespace ApiGateway.Common
{
    public class BaseContract
    {
        public Guid OperationId { get; set; } = new Guid();
        public string ApiVersion { get; set; } = String.Empty;
    }
}
