using AIOrchestra.APIGateway.Resources;
using CommonLibrary;
using FluentValidation;
using KafkaLibrary.Interfaces;
using System.Net;

namespace AIOrchestra.APIGateway.Shared
{
    public static class APIUtils
    {
        public static async Task<BaseResponse> ExecuteBaseRequest<T>(BaseRequest request, string handlerMethod, IProducer producer, IValidator<T> validator)
        {
            var (hasError, baseResponse) = ValidateRequest(request, validator);
            if (hasError)
            {
                return baseResponse!;
            }
            request.HandlerMethod = handlerMethod;
            var response = await producer.ProduceAsync(request.TargetTopic, request.OperationId, request);
            return response;
        }

        public static (bool hasError, BaseResponse? baseResponse) ValidateRequest<T>(BaseRequest request, IValidator<T> validator)
        {
            var validationResult = SharedLibrary.ValidationHelper.ValidateRequest(validator, request.Value);
            if (!string.IsNullOrEmpty(validationResult))
            {
                return (true, SharedLibrary.ApplicationResponseUtils.GenerateResponse(
                    request.OperationId,
                    request.ApiVersion,
                    false,
                    HttpStatusCode.BadRequest,
                    ApplicationErrors.InvalidRequest_NullFields,
                    validationResult,
                    null,
                    null,
                    request.TargetTopic,
                    null,
                    request.Value,
                    request.HandlerMethod));
            }
            return (false, null);
        }
    }
}
