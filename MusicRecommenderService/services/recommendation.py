import numpy as np
import pandas as pd
from sklearn.preprocessing import MinMaxScaler
from sklearn.metrics.pairwise import cosine_similarity
import tabulate
from config.config import MUSIC_DS_PATH, RECOMMENDED_FEATURE_COLS, MUSIC_FEATURE_COLS
import utils.data_processing as data_processing

music_data = pd.read_csv(MUSIC_DS_PATH)
music_data = data_processing.process_music_dataset(music_data)

def recommend(user_preferences, user_songs=None, top_n=15):
    user_preference_vector = np.array([[
            user_preferences['danceability'],
            user_preferences['energy'],
            user_preferences['loudness'],
            user_preferences['speechiness'],
            user_preferences['instrumentalness'],
            user_preferences['liveness']
        ]])

    # Normalize the user preference vector to match the normalization applied to the dataset
    scaler = MinMaxScaler()
    user_preference_vector = scaler.fit_transform(user_preference_vector.reshape(-1, 1)).reshape(1, -1)

    # Calculate similarity based on user preferences
    preferences_similarity = cosine_similarity(user_preference_vector, music_data[MUSIC_FEATURE_COLS])
    
    # Calculate similarity based on user songs if provided
    if user_songs:
        user_songs_features = music_data[music_data['track_id'].isin(user_songs)][MUSIC_FEATURE_COLS]
        user_songs_mean_features = user_songs_features.mean().values.reshape(1, -1)
        user_songs_similarity = cosine_similarity(user_songs_mean_features, music_data[MUSIC_FEATURE_COLS])
        
        # Combine the similarities by averaging them
        combined_similarity = (preferences_similarity + user_songs_similarity) / 2
    else:
        combined_similarity = preferences_similarity

    top_n_indices = np.argsort(combined_similarity[0])[-top_n:][::-1]
    recommended_songs = music_data.iloc[top_n_indices]

    print("Recommended Songs:")
    print(tabulate.tabulate(recommended_songs[RECOMMENDED_FEATURE_COLS], headers='keys', tablefmt='pretty'))

    return recommended_songs[RECOMMENDED_FEATURE_COLS]