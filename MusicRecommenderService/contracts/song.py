class Song:
    def __init__(self, track_id, name, artist, spotify_preview_url, genre, year, duration_ms):
        self.track_id = track_id
        self.name = name
        self.artist = artist
        self.spotify_preview_url = spotify_preview_url
        self.genre = genre
        self.year = year
        self.duration_ms = duration_ms

    @staticmethod
    def from_dict(data):
        return Song(
            track_id=data.get('track_id'),
            name=data.get('name'),
            artist=data.get('artist'),
            spotify_preview_url=data.get('spotify_preview_url'),
            genre=data.get('genre'),
            year=data.get('year'),
            duration_ms=data.get('duration_ms')
        )

    def to_dict(self):
        return {
            'track_id': self.track_id,
            'name': self.name,
            'artist': self.artist,
            'spotify_preview_url': self.spotify_preview_url,
            'genre': self.genre,
            'year': self.year,
            'duration_ms': self.duration_ms
        }