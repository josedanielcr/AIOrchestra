from confluent_kafka import Consumer, KafkaException, KafkaError, Producer
from config import get_kafka_config
from contracts.baseResponse import Error
import utils.internal as internal
from config.config import KAFKA_CONSUME_TOPIC
import json

# Kafka Configuration
kafka_config = get_kafka_config()

# Initialize Kafka Consumer
consumer = Consumer(kafka_config)
consumer.subscribe([KAFKA_CONSUME_TOPIC])

def kafka_consumer_loop():
    while True:
        try:
            msg = consumer.poll(timeout=1.0)
            if msg is not None:
                if msg.error():
                    if msg.error().code() != KafkaError._PARTITION_EOF:
                        print(f"Error: {msg.error()}")
                else:
                    # obtains the method
                    message = json.loads(msg.value().decode('utf-8'))
                    print(f"Received message: {message}")
                    method = internal.get_internal_method_by_name(message['HandlerMethod'])
                    baseResponse = internal.generate_base_response(message)
                    try:
                        response = method(message, message['Songs'])
                        baseResponse.value = response
                        baseResponse.IsSuccess = True
                        baseResponse.IsFailure = False
                        baseResponse.StatusCode = 200
                    except Exception as e:
                        baseResponse.IsSuccess = False
                        baseResponse.IsFailure = True
                        baseResponse.StatusCode = 500
                        baseResponse.Error.Message = f"Error: {e}"
                        baseResponse.Error.Code = 500
                        baseResponse.Error.Details = None
                    finally:
                        produce_message('ApiGatewayResponse', key=baseResponse.operation_id, value=baseResponse)
        except KafkaException as e:
            print(f"Kafka exception: {e}")

def produce_message(topic, key, value):
    serialized_key = key.encode('utf-8')
    serialized_value = json.dumps(value).encode('utf-8')
    Producer.produce(topic, key=serialized_key, value=serialized_value)
    Producer.flush()