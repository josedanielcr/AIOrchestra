import os
from config.config import DOCKER_HOST, DOCKER_PORT, DOCKER_SSL_CERT, DOCKER_SSL_KEY, LOCAL_HOST, LOCAL_PORT, LOCAL_SSL_CERT, LOCAL_SSL_KEY

def get_server_config():
    if os.getenv('RUNNING_IN_DOCKER', 'false').lower() == 'true':
        return {
            'host': DOCKER_HOST,
            'port': DOCKER_PORT,
            'ssl_context': (
                DOCKER_SSL_CERT,
                DOCKER_SSL_KEY
            )
        }
    else:
        return {
            'host': LOCAL_HOST,
            'port': LOCAL_PORT,
            'ssl_context': (
                LOCAL_SSL_CERT,
                LOCAL_SSL_KEY
            )
        }