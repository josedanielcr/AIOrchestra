import pandas as pd
import data.loader as loader
import data.scaler as scaler
from config.config import USER_PLAYCOUNT_COLS, USER_PLAY_COUNT_PATH

def process():
    user_playcount_data = loader.load_data(USER_PLAY_COUNT_PATH)
    return user_playcount_data

def merge_user_playcount_data(user_playcount_data, music_data):
    return pd.merge(user_playcount_data, music_data, left_on='track_id', right_on='track_id')

def normalize_playcounts(merged_data):
    merged_data['normalized_playcount'] = scaler.scale(merged_data, USER_PLAYCOUNT_COLS)
    return merged_data