using CommonLibrary;
using System.Net;

namespace SharedLibrary
{
    public static class InvokeMethodHelper
    {
        private const string MethodInvokeErrorCode = "METHOD_INVOKE_ERROR";

        public static async Task<BaseResponse> InvokeMethod(BaseRequest baseRequest)
        {
            try
            {
                Type type = Type.GetType(baseRequest.HandlerMethod, throwOnError: true)! ?? throw new Exception("Handler type not found");

                var method = type.GetMethod(baseRequest.HandlerMethod) ?? throw new Exception("Method not found");

                var instance = Activator.CreateInstance(type);

                var result = method.Invoke(instance, new object[] { baseRequest }) as Task<BaseResponse> ?? throw new Exception("Method did not return Task<BaseResponse>");

                return await result;
            }
            catch (Exception e)
            {
                return GenerateApplicationResponse.GenerateResponse(
                        baseRequest.OperationId,
                        baseRequest.ApiVersion,
                        false,
                        HttpStatusCode.BadRequest,
                        MethodInvokeErrorCode,
                        e.Message,
                        null,
                        null,
                        baseRequest.TargetTopic,
                        null);
            }
        }
    }
}