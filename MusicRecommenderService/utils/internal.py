
import importlib

from contracts.baseResponse import BaseResponse

def get_internal_method_by_name(name):
    servicename, methodname = name.split('.')
    service_module = importlib.import_module(f'services.{servicename}')
    return getattr(service_module, methodname)

def generate_base_response(request):
    response = BaseResponse()
    response.operation_id = request['OperationId']
    response.api_version = request['ApiVersion']
    response.serviced_by = None
    response.handler_method = request['HandlerMethod']
    response.processing_time = 0
    response.additional_details = None
    response.value = None
    return response