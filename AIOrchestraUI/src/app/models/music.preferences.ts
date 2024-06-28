import { Song } from "./song";

export class MusicPreferences {
    danceability: number;
    energy: number;
    loudness: number;
    speechiness: number;
    instrumentalness: number;
    liveness: number;
    songs: Song[];

    constructor(
        danceability: number,
        energy: number,
        loudness: number,
        speechiness: number,
        instrumentalness: number,
        liveness: number,
        songs: Song[]
    ) {
        this.danceability = danceability;
        this.energy = energy;
        this.loudness = loudness;
        this.speechiness = speechiness;
        this.instrumentalness = instrumentalness;
        this.liveness = liveness;
        this.songs = songs;
    }
}