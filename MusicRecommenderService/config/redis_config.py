import os
from config.config import REDIS_HOST, REDIS_PORT, REDIS_PASSWORD, REDIS_HOST_DOCKER

def get_redis_config():
    if os.getenv('RUNNING_IN_DOCKER', 'false').lower() == 'true':
        return {
            'host': REDIS_HOST_DOCKER,
            'port': REDIS_PORT,
            'password': REDIS_PASSWORD
        }
    else:
        return {
            'host': REDIS_HOST,
            'port': REDIS_PORT,
            'password': REDIS_PASSWORD
        }