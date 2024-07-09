import pandas as pd
from config.config import MUSIC_DS_PATH
import utils.data_processing as data_processing

music_data = pd.read_csv(MUSIC_DS_PATH)
music_data = data_processing.process_music_dataset(music_data)

def get_tracks_by_ids(baseRequest, songs = None):
    return music_data[music_data['track_id'].isin(baseRequest['Value']['TrackIds'])]