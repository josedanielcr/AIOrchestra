namespace AIOrchestra.APIGateway.Common.Entities
{
    public class BaseResponse : BaseContract
    {
        public bool IsSuccess { get; set; } = true;
        public bool IsFailure { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}
