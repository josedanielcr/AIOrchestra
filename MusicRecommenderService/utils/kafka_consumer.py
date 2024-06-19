from confluent_kafka import Consumer, KafkaException, KafkaError
from config import get_kafka_config

# Kafka Configuration
kafka_config = get_kafka_config()

# Initialize Kafka Consumer
consumer = Consumer(kafka_config)
consumer.subscribe(['musicRecommender'])

def kafka_consumer_loop():
    while True:
        try:
            msg = consumer.poll(timeout=1.0)
            if msg is not None:
                if msg.error():
                    if msg.error().code() != KafkaError._PARTITION_EOF:
                        print(f"Error: {msg.error()}")
                else:
                    print(f"Consumed message: {msg.value().decode('utf-8')}")
        except KafkaException as e:
            print(f"Kafka exception: {e}")