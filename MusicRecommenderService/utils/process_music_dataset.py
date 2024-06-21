import data.loader as loader
import data.scaler as scaler
from config.config import MUSIC_DS_PATH, MUSIC_PLAYCOUNT_FEATURE_COLS

def process():
    music_data = loader.load_data(MUSIC_DS_PATH)
    return music_data

def normalize_music_data(df):
    df[MUSIC_PLAYCOUNT_FEATURE_COLS] = scaler.scale(df,MUSIC_PLAYCOUNT_FEATURE_COLS)
    return df