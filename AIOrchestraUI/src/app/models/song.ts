export class Song {
    track_id: string;
    name: string;
    artist: string;
    spotify_preview_url: string;
    genre: string;
    year: number;
    duration_ms: number;

    constructor(
        track_id: string,
        name: string,
        artist: string,
        spotify_preview_url: string,
        genre: string,
        year: number,
        duration_ms: number
    ) {
        this.track_id = track_id;
        this.name = name;
        this.artist = artist;
        this.spotify_preview_url = spotify_preview_url;
        this.genre = genre;
        this.year = year;
        this.duration_ms = duration_ms;
    }
}