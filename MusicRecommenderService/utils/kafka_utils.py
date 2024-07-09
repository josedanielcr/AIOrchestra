import math
from confluent_kafka import Consumer, KafkaException, KafkaError, Producer
from flask import jsonify
from config import get_kafka_config
from contracts.baseResponse import BaseResponse, Error
from contracts.song import Song
import utils.internal as internal
from config.config import KAFKA_CONSUME_TOPIC
import json

# Kafka Configuration
kafka_config = get_kafka_config()

# Initialize Kafka Consumer
consumer = Consumer(kafka_config)
consumer.subscribe([KAFKA_CONSUME_TOPIC])

# Initialize Kafka Producer
Producer = Producer(kafka_config)

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
                    method = internal.get_internal_method_by_name(message['HandlerMethod'])
                    baseResponse = internal.generate_base_response(message)
                    try:
                        response = method(message, message['Songs'] if 'Songs' in message and message['Songs'] is not None else None)
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

def produce_message(topic, key, value : BaseResponse):

    # for testing create a song object with some dummy data
    serialized_key = key.encode('utf-8')
    value.serviced_by = 5
    value.from_dataframe(value.value)
    for song in value.value:
        if(type(song.genre) == str):
            continue
        if(math.isnan(song.genre)):
            song.genre = "Unknown"
    # value.value = None
    value = value.to_dict()
    value = json.dumps(value)
    print(f"Producing message" + key)
    Producer.produce(topic, key=serialized_key, value=value)
    Producer.flush()