from confluent_kafka import Consumer, KafkaException, KafkaError, Producer
from config import get_kafka_config
from contracts.baseResponse import Error
import internal

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
                    # obtains the method
                    print(f"Consumed message: {msg.value().decode('utf-8')}")
                    method = internal.get_internal_method_by_name(msg.value().decode('utf-8').methodName)
                    baseResponse = internal.generate_baseResponse(msg.value().decode('utf-8'))
                    try:
                        response = method(msg.value().decode('utf-8').Value)
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
    Producer.produce(topic, key=key, value=value)
    Producer.flush()