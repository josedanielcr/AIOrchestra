from flask import Flask, jsonify
from services.training import train_model
from utils.kafka_consumer import kafka_consumer_loop
from config import get_server_config
from dotenv import load_dotenv
import threading

load_dotenv()

app = Flask(__name__)

@app.route('/train', methods=['POST'])
def train():
    message = train_model()
    return jsonify({"message": message})

if __name__ == '__main__':
    # Start the Kafka consumer thread
    consumer_thread = threading.Thread(target=kafka_consumer_loop)
    consumer_thread.daemon = True
    consumer_thread.start()

    server_config = get_server_config()
    app.run(host=server_config['host'], port=server_config['port'], ssl_context=server_config['ssl_context'])