import os

def get_server_config():
    if os.getenv('RUNNING_IN_DOCKER', 'false').lower() == 'true':
        return {
            'host': '0.0.0.0',
            'port': 8088,
            'ssl_context': (
                '/app/certificates/server.crt',
                '/app/certificates/server.key'
            )
        }
    else:
        return {
            'host': '0.0.0.0',
            'port': 8088,
            'ssl_context': (
                './certificates/server.crt',
                './certificates/server.key'
            )
        }
