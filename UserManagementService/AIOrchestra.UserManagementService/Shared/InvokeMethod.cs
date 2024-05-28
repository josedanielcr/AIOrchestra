using AIOrchestra.UserManagementService.Configurations;
using CommonLibrary;

namespace AIOrchestra.UserManagementService.Shared
{
    public static class InvokeMethod
    {
        public static async Task InvokeMethodAsync(IServiceProvider serviceProvider, string methodName, BaseRequest baseRequest)
        {
            try
            {
                if (!MethodMappingConfiguration.MethodMappings.TryGetValue(methodName, out var fullyQualifiedName))
                {
                    throw new Exception("Method mapping not found");
                }

                var handlerMethodParts = fullyQualifiedName.Split('.');
                if (handlerMethodParts.Length < 2)
                {
                    throw new Exception("Invalid mapped method format. Expected format: Namespace.ClassName.MethodName");
                }

                var namespaceAndClassName = string.Join('.', handlerMethodParts.Take(handlerMethodParts.Length - 1));
                var actualMethodName = handlerMethodParts.Last();

                var type = Type.GetType(namespaceAndClassName) ?? throw new Exception($"Type '{namespaceAndClassName}' could not be found.");

                var method = type.GetMethod(actualMethodName) ?? throw new Exception($"Method '{actualMethodName}' could not be found in type '{namespaceAndClassName}'.");

                var constructor = type.GetConstructors().FirstOrDefault();
                var parameters = constructor?.GetParameters()
                    .Select(p => serviceProvider.GetRequiredService(p.ParameterType))
                    .ToArray();

                var instance = Activator.CreateInstance(type, parameters) ?? throw new Exception($"Instance of type '{namespaceAndClassName}' could not be created.");

                var result = method.Invoke(instance, new object[] { baseRequest }) as Task;
                if (result != null)
                {
                    await result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error invoking method: {e.Message}");
            }
        }
    }
}
