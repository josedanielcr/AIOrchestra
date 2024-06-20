import data.loader as loader
import data.scaler as scaler
from config.config import MUSIC_FEATURE_COLS, MUSIC_DS_PATH

def process():
    # Load and preprocess music data
    music_data = loader.load_data(MUSIC_DS_PATH)
    X = scaler.scale(music_data, MUSIC_FEATURE_COLS)
    return X