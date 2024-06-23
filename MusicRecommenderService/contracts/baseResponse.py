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

class Error:
    def __init__(self):
        self.code: str = ""
        self.message: str = ""
        self.details: str = ""