using CommonLibrary;
using System.Net;

namespace AIOrchestra.UserManagementService.Shared
{
    public static class GenerateBaseResponse
    {
        public static BaseResponse GenerateBaseResponseSync(BaseRequest request)
        {
            return new BaseResponse
            {
                OperationId = request.OperationId,
                ApiVersion = request.ApiVersion,
                ServicedBy = Topics.UserManagement,
                HandlerMethod = request.HandlerMethod,
                ProcessingTime = 0,
                AdditionalDetails = null,
                Value = null
            };
        }
    }
}
