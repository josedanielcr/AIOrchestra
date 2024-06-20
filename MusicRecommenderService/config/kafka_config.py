import os

from config.config import KAFKA_DOCKER_BOOTSTRAP_SERVERS, KAFKA_LOCAL_BOOTSTRAP_SERVERS, KAFKA_GROUP_ID, KAFKA_AUTO_OFFSET_RESET

def get_kafka_config():
    if os.getenv('RUNNING_IN_DOCKER', 'false').lower() == 'true':
        return {
            'bootstrap.servers': KAFKA_DOCKER_BOOTSTRAP_SERVERS,
            'group.id': KAFKA_GROUP_ID,
            'auto.offset.reset': KAFKA_AUTO_OFFSET_RESET
        }
    else:
        return {
            'bootstrap.servers': KAFKA_LOCAL_BOOTSTRAP_SERVERS,
            'group.id': KAFKA_GROUP_ID,
            'auto.offset.reset': KAFKA_AUTO_OFFSET_RESET
        }