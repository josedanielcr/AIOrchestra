import os

def get_kafka_config():
    if os.getenv('RUNNING_IN_DOCKER', 'false').lower() == 'true':
        return {
            'bootstrap.servers': 'broker-1:29092,broker-2:29093,broker-3:29094',
            'group.id': 'music-recommender-group',
            'auto.offset.reset': 'earliest'
        }
    else:
        return {
            'bootstrap.servers': 'localhost:9092,localhost:9093,localhost:9094',
            'group.id': 'music-recommender-group',
            'auto.offset.reset': 'earliest'
        }