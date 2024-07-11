import json
import uuid
from typing import Any, List
from contracts.baseContactStatus import StatusCode

import pandas as pd

from contracts.song import Song

class BaseContract:
    def __init__(self):
        self.operation_id: str = str(uuid.uuid4())
        self.api_version: str = ""
        self.value: Any = None
        self.handler_method: str = ""
        self.status : StatusCode = StatusCode.Pending

    def to_dict(self):
        return {
            "OperationId": self.operation_id,
            "ApiVersion": self.api_version,
            "Value": [song.to_dict() for song in self.value],
            "HandlerMethod": self.handler_method,
            "Status": self.status.name
        }
    
    def from_dataframe(self, df: pd.DataFrame):
        self.value = [Song.from_dict(row) for row in df.to_dict(orient='records')]