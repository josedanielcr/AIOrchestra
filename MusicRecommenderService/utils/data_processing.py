import pandas as pd
import numpy as np
from sklearn.preprocessing import MinMaxScaler
from config.config import MUSIC_FEATURE_COLS, USER_PLAYCOUNT_COLS


def process_music_dataset(music_data):
    scaler = MinMaxScaler()
    music_features = music_data[MUSIC_FEATURE_COLS]
    music_data[music_features.columns] = scaler.fit_transform(music_features)
    return music_data