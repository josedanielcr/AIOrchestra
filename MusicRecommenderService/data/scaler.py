from sklearn.preprocessing import MinMaxScaler

def scale(music_data, feature_cols):
    scaler = MinMaxScaler()
    X = scaler.fit_transform(music_data[feature_cols])
    return X