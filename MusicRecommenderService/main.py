from flask import Flask, jsonify, request
from services.recommendation import recommend
from utils.kafka_consumer import kafka_consumer_loop
from config import get_server_config
from dotenv import load_dotenv
import threading

load_dotenv()

app = Flask(__name__)
@app.route('/recommend', methods=['POST'])
def get_recommendation():
    data = request.json
    user_preferences = {
        "danceability": data["danceability"],
        "energy": data["energy"],
        "loudness": data["loudness"],
        "speechiness": data["speechiness"],
        "instrumentalness": data["instrumentalness"],
        "liveness": data["liveness"]
    }
    user_history = data["songs"]
    recommendations = recommend(user_preferences, user_history)
    return jsonify(recommendations.to_dict(orient='records'))

if __name__ == '__main__':
    # Start the Kafka consumer thread
    consumer_thread = threading.Thread(target=kafka_consumer_loop)
    consumer_thread.daemon = True
    consumer_thread.start()

    server_config = get_server_config()
    app.run(host=server_config['host'], port=server_config['port'], ssl_context=server_config['ssl_context'])