from typing import Dict, Any
from contracts.baseContract import BaseContract

class BaseResponse(BaseContract):
    def __init__(self):
        super().__init__()
        self.is_success: bool = True
        self.is_failure: bool = False
        self.status_code: int = 200
        self.error: Error = Error()
        self.processing_time: int = 0
        self.serviced_by: Any = None
        self.additional_details: Dict[str, str] = {}
    
    def to_dict(self):
        parent_dict = super().to_dict()
        return {
            **parent_dict,
            "IsSuccess": self.is_success,
            "IsFailure": self.is_failure,
            "StatusCode": self.status_code,
            "Error": self.error.__dict__,
            "ProcessingTime": self.processing_time,
            "ServicedBy": self.serviced_by,
            "AdditionalDetails": self.additional_details
        }

class Error:
    def __init__(self):
        self.code: str = ""
        self.message: str = ""
        self.details: str = ""