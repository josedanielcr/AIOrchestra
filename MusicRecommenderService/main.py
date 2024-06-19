from flask import Flask, jsonify
from utils.kafka_consumer import kafka_consumer_loop
from config import get_server_config
from dotenv import load_dotenv
import threading

# Load environment variables from .env file
load_dotenv()

app = Flask(__name__)

@app.route('/')
def hello_world():
    return jsonify(message="Hello, HTTPS World!")

if __name__ == '__main__':
    # Start the Kafka consumer thread
    consumer_thread = threading.Thread(target=kafka_consumer_loop)
    consumer_thread.daemon = True
    consumer_thread.start()

    server_config = get_server_config()
    app.run(host=server_config['host'], port=server_config['port'], ssl_context=server_config['ssl_context'])
