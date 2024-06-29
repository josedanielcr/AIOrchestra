using CommonLibrary;
using System.Net;

namespace SharedLibrary
{
    public static class ApplicationResponseUtils
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
            Dictionary<string, string>? AdditionalDetails,
            object value,
            string HandlerMethod)
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
                },
                Value = value,
                HandlerMethod = HandlerMethod
            };
            return response;
        }

        public static BaseResponse AddErrorResultToResponse(BaseResponse response, Exception e)
        {
            response.IsSuccess = false;
            response.IsFailure = true;
            response.StatusCode = HttpStatusCode.InternalServerError;
            response.Error = new Error
            {
                Code = HttpStatusCode.InternalServerError.ToString(),
                Message = e.Message,
                Details = String.Empty
            };
            return response;
        }

        public static BaseResponse AddSuccessResultToResponse(BaseResponse response, object value)
        {
            response.IsSuccess = true;
            response.IsFailure = false;
            response.StatusCode = HttpStatusCode.OK;
            response.Value = value;
            return response;
        }
    }
}