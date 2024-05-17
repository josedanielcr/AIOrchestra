using CommonLibrary;
using System.Net;

namespace SharedLibrary
{
    public static class GenerateApplicationResponse
    {
        public static BaseResponse GenerateResponse(
            string operationId,
            string apiVersion,
            System.Boolean isSuccess,
            HttpStatusCode status,
            string? code,
            string? message,
            object? details,
            long? ProcessingTime,
            Topics? ServicedBy,
            Dictionary<string, string>? AdditionalDetails)
        {
            BaseResponse response = new BaseResponse
            {
                OperationId = operationId,
                ApiVersion = apiVersion,
                IsSuccess = isSuccess,
                IsFailure = !isSuccess,
                StatusCode = status,
                ProcessingTime = ProcessingTime ?? 0,
                ServicedBy = ServicedBy ?? Topics.Unknown,
                AdditionalDetails = AdditionalDetails ?? new Dictionary<string, string>(),
                Error = new CommonLibrary.Error
                {
                    Code = code ?? string.Empty,
                    Message = message ?? string.Empty,
                    Details = details?.ToString() ?? string.Empty
                }
            };
            return response;
        }
    }
}