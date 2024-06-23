import uuid
from typing import Any

class BaseContract:
    def __init__(self):
        self.operation_id: str = str(uuid.uuid4())
        self.api_version: str = ""
        self.value: Any = None
        self.handler_method: str = ""