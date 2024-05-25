using CommonLibrary;
using System.Net;
using System.Reflection;

namespace SharedLibrary
{
    public static class InvokeMethodHelper
    {
        private const string MethodInvokeErrorCode = "METHOD_INVOKE_ERROR";
        private static Dictionary<string, string> methodMappings = new Dictionary<string, string>();

        public static void Initialize(Dictionary<string, string> mappings)
        {
            methodMappings = mappings;
        }

        public static async Task<BaseResponse> InvokeMethod(BaseRequest baseRequest)
        {
            try
            {
                string assemblyQualifiedName, methodName;
                ValidateMethodParam(baseRequest, out assemblyQualifiedName, out methodName);

                Type type = Type.GetType(assemblyQualifiedName) ?? throw new Exception("Handler type not found");
                MethodInfo? method = type.GetMethod(methodName) ?? throw new Exception("Method not found");
                object instance = Activator.CreateInstance(type) ?? throw new Exception("Failed to create an instance of the handler type");

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

        private static void ValidateMethodParam(BaseRequest baseRequest, out string assemblyQualifiedName, out string methodName)
        {
            if (!methodMappings.TryGetValue(baseRequest.HandlerMethod, out var fullyQualifiedName))
            {
                throw new Exception("Method mapping not found");
            }

            var parts = fullyQualifiedName.Split(new[] { ',' }, 2);
            if (parts.Length != 2)
            {
                throw new Exception("Invalid mapped method format. Expected format: Namespace.ClassName.MethodName, AssemblyName");
            }

            var typeAndMethod = parts[0];
            var assemblyName = parts[1].Trim();

            var handlerMethodParts = typeAndMethod.Split('.');
            if (handlerMethodParts.Length < 3)
            {
                throw new Exception("Invalid mapped method format. Expected format: Namespace.ClassName.MethodName");
            }

            var namespaceAndClassName = string.Join('.', handlerMethodParts.Take(handlerMethodParts.Length - 1));
            methodName = handlerMethodParts.Last();

            assemblyQualifiedName = $"{namespaceAndClassName}, {assemblyName}";
        }
    }
}