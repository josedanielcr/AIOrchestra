import numpy as np
import pandas as pd
from keras.api.models import load_model
from config.config import MUSIC_DS_PATH, RECOMMENDED_FEATURE_COLS

music_data = pd.read_csv(MUSIC_DS_PATH)
model = load_model('music_recommender_model.h5')

def recommend(user_preferences, top_n=15):
    user_preference_vector = np.array([[
        user_preferences['danceability'],
        user_preferences['energy'],
        user_preferences['loudness'],
        user_preferences['speechiness'],
        user_preferences['instrumentalness'],
        user_preferences['liveness']
    ]])
    
    predictions = model.predict(user_preference_vector)

    # Get top N recommendations
    top_n_indices = np.argsort(predictions[0])[-top_n:][::-1]
    recommended_songs = music_data.iloc[top_n_indices]

    return recommended_songs[RECOMMENDED_FEATURE_COLS]