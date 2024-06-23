from datetime import datetime
import uuid
from typing import Dict, Any
from contracts.baseContract import BaseContract

class BaseRequest(BaseContract):
    def __init__(self):
        super().__init__()
        self.timestamp: datetime = datetime.utcnow()
        self.requester_id: str = ""
        self.correlation_id: str = str(uuid.uuid4())
        self.headers: Dict[str, str] = {}
        self.target_topic: Any = None
        self.status: int = None