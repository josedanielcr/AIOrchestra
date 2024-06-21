from models.model import create_model
from config.config import MUSIC_FEATURE_COLS, USER_PLAYCOUNT_COLS
import utils.process_music_dataset as process_music_dataset
import utils.process_user_playcount_dataset as process_user_playcount_dataset
from keras.api.callbacks import Callback

class TrainingLogger(Callback):
    def on_epoch_end(self, epoch, logs=None):
        logs = logs or {}
        print(f"Epoch {epoch+1}/{self.params['epochs']}")
        print(f" - loss: {logs.get('loss'):.4f} - val_loss: {logs.get('val_loss'):.4f}")
        print(f" - metrics: {logs}")

def train_model():
    print("Starting model training...")

    print("Processing music dataset...")
    X = process_music_dataset.process()

    print("Processing user playcount dataset...")
    user_playcount_data = process_user_playcount_dataset.process()

    print("Merging datasets...")
    merged_data = process_user_playcount_dataset.merge_user_playcount_data(user_playcount_data, X)

    print("Normalizing music and user playcount data...")
    merged_data = process_music_dataset.normalize_music_data(merged_data)

    X = merged_data[MUSIC_FEATURE_COLS].values
    Y = merged_data[USER_PLAYCOUNT_COLS].values

    print("Creating model...")
    model = create_model(input_dim=X.shape[1])
    print("Training model...")
    model.fit(
        X, Y,
        epochs=4,
        batch_size=32,
        validation_split=0.2,
        verbose=1, 
        callbacks=[TrainingLogger()]
    )

    print("Saving model...")
    model.save('music_recommender_model.h5')
    print("Model training complete and saved as 'music_recommender_model.h5'.")