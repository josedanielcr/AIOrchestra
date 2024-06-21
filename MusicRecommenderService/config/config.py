# Kafka
KAFKA_DOCKER_BOOTSTRAP_SERVERS = 'broker-1:29092,broker-2:29093,broker-3:29094' 
KAFKA_LOCAL_BOOTSTRAP_SERVERS = 'localhost:9092,localhost:9093,localhost:9094'
KAFKA_GROUP_ID = 'music-recommender-group'
KAFKA_AUTO_OFFSET_RESET = 'earliest'

# Server
DOCKER_HOST = '0.0.0.0'
DOCKER_PORT = 8088
DOCKER_SSL_CERT = '/app/certificates/server.crt'
DOCKER_SSL_KEY = '/app/certificates/server.key'
LOCAL_HOST = '0.0.0.0'
LOCAL_PORT = 8088
LOCAL_SSL_CERT = './certificates/server.crt'
LOCAL_SSL_KEY = './certificates/server.key'

# ML model
MUSIC_FEATURE_COLS = ["danceability", "energy", "loudness", "speechiness", "instrumentalness", "liveness"]
USER_PLAYCOUNT_COLS = ['playcount']
MUSIC_PLAYCOUNT_FEATURE_COLS = ["danceability", "energy", "loudness", "speechiness", "instrumentalness", "liveness", "playcount"]
MUSIC_DS_PATH = './data/music_data.csv'
USER_PLAY_COUNT_PATH = './data/user_playcount_data.csv'
RECOMMENDED_FEATURE_COLS = ['track_id', 'name', 'artist','spotify_preview_url', 'genre', 'year' ,'duration_ms']